
// File: @openzeppelin/contracts/utils/math/SignedMath.sol


// OpenZeppelin Contracts (last updated v4.8.0) (utils/math/SignedMath.sol)

pragma solidity ^0.8.0;

/**
 * @dev Standard signed math utilities missing in the Solidity language.
 */
library SignedMath {
    /**
     * @dev Returns the largest of two signed numbers.
     */
    function max(int256 a, int256 b) internal pure returns (int256) {
        return a > b ? a : b;
    }

    /**
     * @dev Returns the smallest of two signed numbers.
     */
    function min(int256 a, int256 b) internal pure returns (int256) {
        return a < b ? a : b;
    }

    /**
     * @dev Returns the average of two signed numbers without overflow.
     * The result is rounded towards zero.
     */
    function average(int256 a, int256 b) internal pure returns (int256) {
        // Formula from the book "Hacker's Delight"
        int256 x = (a & b) + ((a ^ b) >> 1);
        return x + (int256(uint256(x) >> 255) & (a ^ b));
    }

    /**
     * @dev Returns the absolute unsigned value of a signed value.
     */
    function abs(int256 n) internal pure returns (uint256) {
        unchecked {
            // must be unchecked in order to support `n = type(int256).min`
            return uint256(n >= 0 ? n : -n);
        }
    }
}

// File: @openzeppelin/contracts/utils/math/Math.sol


// OpenZeppelin Contracts (last updated v4.9.0) (utils/math/Math.sol)

pragma solidity ^0.8.0;

/**
 * @dev Standard math utilities missing in the Solidity language.
 */
library Math {
    enum Rounding {
        Down, // Toward negative infinity
        Up, // Toward infinity
        Zero // Toward zero
    }

    /**
     * @dev Returns the largest of two numbers.
     */
    function max(uint256 a, uint256 b) internal pure returns (uint256) {
        return a > b ? a : b;
    }

    /**
     * @dev Returns the smallest of two numbers.
     */
    function min(uint256 a, uint256 b) internal pure returns (uint256) {
        return a < b ? a : b;
    }

    /**
     * @dev Returns the average of two numbers. The result is rounded towards
     * zero.
     */
    function average(uint256 a, uint256 b) internal pure returns (uint256) {
        // (a + b) / 2 can overflow.
        return (a & b) + (a ^ b) / 2;
    }

    /**
     * @dev Returns the ceiling of the division of two numbers.
     *
     * This differs from standard division with `/` in that it rounds up instead
     * of rounding down.
     */
    function ceilDiv(uint256 a, uint256 b) internal pure returns (uint256) {
        // (a + b - 1) / b can overflow on addition, so we distribute.
        return a == 0 ? 0 : (a - 1) / b + 1;
    }

    /**
     * @notice Calculates floor(x * y / denominator) with full precision. Throws if result overflows a uint256 or denominator == 0
     * @dev Original credit to Remco Bloemen under MIT license (https://xn--2-umb.com/21/muldiv)
     * with further edits by Uniswap Labs also under MIT license.
     */
    function mulDiv(uint256 x, uint256 y, uint256 denominator) internal pure returns (uint256 result) {
        unchecked {
            // 512-bit multiply [prod1 prod0] = x * y. Compute the product mod 2^256 and mod 2^256 - 1, then use
            // use the Chinese Remainder Theorem to reconstruct the 512 bit result. The result is stored in two 256
            // variables such that product = prod1 * 2^256 + prod0.
            uint256 prod0; // Least significant 256 bits of the product
            uint256 prod1; // Most significant 256 bits of the product
            assembly {
                let mm := mulmod(x, y, not(0))
                prod0 := mul(x, y)
                prod1 := sub(sub(mm, prod0), lt(mm, prod0))
            }

            // Handle non-overflow cases, 256 by 256 division.
            if (prod1 == 0) {
                // Solidity will revert if denominator == 0, unlike the div opcode on its own.
                // The surrounding unchecked block does not change this fact.
                // See https://docs.soliditylang.org/en/latest/control-structures.html#checked-or-unchecked-arithmetic.
                return prod0 / denominator;
            }

            // Make sure the result is less than 2^256. Also prevents denominator == 0.
            require(denominator > prod1, "Math: mulDiv overflow");

            ///////////////////////////////////////////////
            // 512 by 256 division.
            ///////////////////////////////////////////////

            // Make division exact by subtracting the remainder from [prod1 prod0].
            uint256 remainder;
            assembly {
                // Compute remainder using mulmod.
                remainder := mulmod(x, y, denominator)

                // Subtract 256 bit number from 512 bit number.
                prod1 := sub(prod1, gt(remainder, prod0))
                prod0 := sub(prod0, remainder)
            }

            // Factor powers of two out of denominator and compute largest power of two divisor of denominator. Always >= 1.
            // See https://cs.stackexchange.com/q/138556/92363.

            // Does not overflow because the denominator cannot be zero at this stage in the function.
            uint256 twos = denominator & (~denominator + 1);
            assembly {
                // Divide denominator by twos.
                denominator := div(denominator, twos)

                // Divide [prod1 prod0] by twos.
                prod0 := div(prod0, twos)

                // Flip twos such that it is 2^256 / twos. If twos is zero, then it becomes one.
                twos := add(div(sub(0, twos), twos), 1)
            }

            // Shift in bits from prod1 into prod0.
            prod0 |= prod1 * twos;

            // Invert denominator mod 2^256. Now that denominator is an odd number, it has an inverse modulo 2^256 such
            // that denominator * inv = 1 mod 2^256. Compute the inverse by starting with a seed that is correct for
            // four bits. That is, denominator * inv = 1 mod 2^4.
            uint256 inverse = (3 * denominator) ^ 2;

            // Use the Newton-Raphson iteration to improve the precision. Thanks to Hensel's lifting lemma, this also works
            // in modular arithmetic, doubling the correct bits in each step.
            inverse *= 2 - denominator * inverse; // inverse mod 2^8
            inverse *= 2 - denominator * inverse; // inverse mod 2^16
            inverse *= 2 - denominator * inverse; // inverse mod 2^32
            inverse *= 2 - denominator * inverse; // inverse mod 2^64
            inverse *= 2 - denominator * inverse; // inverse mod 2^128
            inverse *= 2 - denominator * inverse; // inverse mod 2^256

            // Because the division is now exact we can divide by multiplying with the modular inverse of denominator.
            // This will give us the correct result modulo 2^256. Since the preconditions guarantee that the outcome is
            // less than 2^256, this is the final result. We don't need to compute the high bits of the result and prod1
            // is no longer required.
            result = prod0 * inverse;
            return result;
        }
    }

    /**
     * @notice Calculates x * y / denominator with full precision, following the selected rounding direction.
     */
    function mulDiv(uint256 x, uint256 y, uint256 denominator, Rounding rounding) internal pure returns (uint256) {
        uint256 result = mulDiv(x, y, denominator);
        if (rounding == Rounding.Up && mulmod(x, y, denominator) > 0) {
            result += 1;
        }
        return result;
    }

    /**
     * @dev Returns the square root of a number. If the number is not a perfect square, the value is rounded down.
     *
     * Inspired by Henry S. Warren, Jr.'s "Hacker's Delight" (Chapter 11).
     */
    function sqrt(uint256 a) internal pure returns (uint256) {
        if (a == 0) {
            return 0;
        }

        // For our first guess, we get the biggest power of 2 which is smaller than the square root of the target.
        //
        // We know that the "msb" (most significant bit) of our target number `a` is a power of 2 such that we have
        // `msb(a) <= a < 2*msb(a)`. This value can be written `msb(a)=2**k` with `k=log2(a)`.
        //
        // This can be rewritten `2**log2(a) <= a < 2**(log2(a) + 1)`
        // → `sqrt(2**k) <= sqrt(a) < sqrt(2**(k+1))`
        // → `2**(k/2) <= sqrt(a) < 2**((k+1)/2) <= 2**(k/2 + 1)`
        //
        // Consequently, `2**(log2(a) / 2)` is a good first approximation of `sqrt(a)` with at least 1 correct bit.
        uint256 result = 1 << (log2(a) >> 1);

        // At this point `result` is an estimation with one bit of precision. We know the true value is a uint128,
        // since it is the square root of a uint256. Newton's method converges quadratically (precision doubles at
        // every iteration). We thus need at most 7 iteration to turn our partial result with one bit of precision
        // into the expected uint128 result.
        unchecked {
            result = (result + a / result) >> 1;
            result = (result + a / result) >> 1;
            result = (result + a / result) >> 1;
            result = (result + a / result) >> 1;
            result = (result + a / result) >> 1;
            result = (result + a / result) >> 1;
            result = (result + a / result) >> 1;
            return min(result, a / result);
        }
    }

    /**
     * @notice Calculates sqrt(a), following the selected rounding direction.
     */
    function sqrt(uint256 a, Rounding rounding) internal pure returns (uint256) {
        unchecked {
            uint256 result = sqrt(a);
            return result + (rounding == Rounding.Up && result * result < a ? 1 : 0);
        }
    }

    /**
     * @dev Return the log in base 2, rounded down, of a positive value.
     * Returns 0 if given 0.
     */
    function log2(uint256 value) internal pure returns (uint256) {
        uint256 result = 0;
        unchecked {
            if (value >> 128 > 0) {
                value >>= 128;
                result += 128;
            }
            if (value >> 64 > 0) {
                value >>= 64;
                result += 64;
            }
            if (value >> 32 > 0) {
                value >>= 32;
                result += 32;
            }
            if (value >> 16 > 0) {
                value >>= 16;
                result += 16;
            }
            if (value >> 8 > 0) {
                value >>= 8;
                result += 8;
            }
            if (value >> 4 > 0) {
                value >>= 4;
                result += 4;
            }
            if (value >> 2 > 0) {
                value >>= 2;
                result += 2;
            }
            if (value >> 1 > 0) {
                result += 1;
            }
        }
        return result;
    }

    /**
     * @dev Return the log in base 2, following the selected rounding direction, of a positive value.
     * Returns 0 if given 0.
     */
    function log2(uint256 value, Rounding rounding) internal pure returns (uint256) {
        unchecked {
            uint256 result = log2(value);
            return result + (rounding == Rounding.Up && 1 << result < value ? 1 : 0);
        }
    }

    /**
     * @dev Return the log in base 10, rounded down, of a positive value.
     * Returns 0 if given 0.
     */
    function log10(uint256 value) internal pure returns (uint256) {
        uint256 result = 0;
        unchecked {
            if (value >= 10 ** 64) {
                value /= 10 ** 64;
                result += 64;
            }
            if (value >= 10 ** 32) {
                value /= 10 ** 32;
                result += 32;
            }
            if (value >= 10 ** 16) {
                value /= 10 ** 16;
                result += 16;
            }
            if (value >= 10 ** 8) {
                value /= 10 ** 8;
                result += 8;
            }
            if (value >= 10 ** 4) {
                value /= 10 ** 4;
                result += 4;
            }
            if (value >= 10 ** 2) {
                value /= 10 ** 2;
                result += 2;
            }
            if (value >= 10 ** 1) {
                result += 1;
            }
        }
        return result;
    }

    /**
     * @dev Return the log in base 10, following the selected rounding direction, of a positive value.
     * Returns 0 if given 0.
     */
    function log10(uint256 value, Rounding rounding) internal pure returns (uint256) {
        unchecked {
            uint256 result = log10(value);
            return result + (rounding == Rounding.Up && 10 ** result < value ? 1 : 0);
        }
    }

    /**
     * @dev Return the log in base 256, rounded down, of a positive value.
     * Returns 0 if given 0.
     *
     * Adding one to the result gives the number of pairs of hex symbols needed to represent `value` as a hex string.
     */
    function log256(uint256 value) internal pure returns (uint256) {
        uint256 result = 0;
        unchecked {
            if (value >> 128 > 0) {
                value >>= 128;
                result += 16;
            }
            if (value >> 64 > 0) {
                value >>= 64;
                result += 8;
            }
            if (value >> 32 > 0) {
                value >>= 32;
                result += 4;
            }
            if (value >> 16 > 0) {
                value >>= 16;
                result += 2;
            }
            if (value >> 8 > 0) {
                result += 1;
            }
        }
        return result;
    }

    /**
     * @dev Return the log in base 256, following the selected rounding direction, of a positive value.
     * Returns 0 if given 0.
     */
    function log256(uint256 value, Rounding rounding) internal pure returns (uint256) {
        unchecked {
            uint256 result = log256(value);
            return result + (rounding == Rounding.Up && 1 << (result << 3) < value ? 1 : 0);
        }
    }
}

// File: @openzeppelin/contracts/utils/Strings.sol


// OpenZeppelin Contracts (last updated v4.9.0) (utils/Strings.sol)

pragma solidity ^0.8.0;



/**
 * @dev String operations.
 */
library Strings {
    bytes16 private constant _SYMBOLS = "0123456789abcdef";
    uint8 private constant _ADDRESS_LENGTH = 20;

    /**
     * @dev Converts a `uint256` to its ASCII `string` decimal representation.
     */
    function toString(uint256 value) internal pure returns (string memory) {
        unchecked {
            uint256 length = Math.log10(value) + 1;
            string memory buffer = new string(length);
            uint256 ptr;
            /// @solidity memory-safe-assembly
            assembly {
                ptr := add(buffer, add(32, length))
            }
            while (true) {
                ptr--;
                /// @solidity memory-safe-assembly
                assembly {
                    mstore8(ptr, byte(mod(value, 10), _SYMBOLS))
                }
                value /= 10;
                if (value == 0) break;
            }
            return buffer;
        }
    }

    /**
     * @dev Converts a `int256` to its ASCII `string` decimal representation.
     */
    function toString(int256 value) internal pure returns (string memory) {
        return string(abi.encodePacked(value < 0 ? "-" : "", toString(SignedMath.abs(value))));
    }

    /**
     * @dev Converts a `uint256` to its ASCII `string` hexadecimal representation.
     */
    function toHexString(uint256 value) internal pure returns (string memory) {
        unchecked {
            return toHexString(value, Math.log256(value) + 1);
        }
    }

    /**
     * @dev Converts a `uint256` to its ASCII `string` hexadecimal representation with fixed length.
     */
    function toHexString(uint256 value, uint256 length) internal pure returns (string memory) {
        bytes memory buffer = new bytes(2 * length + 2);
        buffer[0] = "0";
        buffer[1] = "x";
        for (uint256 i = 2 * length + 1; i > 1; --i) {
            buffer[i] = _SYMBOLS[value & 0xf];
            value >>= 4;
        }
        require(value == 0, "Strings: hex length insufficient");
        return string(buffer);
    }

    /**
     * @dev Converts an `address` with fixed length of 20 bytes to its not checksummed ASCII `string` hexadecimal representation.
     */
    function toHexString(address addr) internal pure returns (string memory) {
        return toHexString(uint256(uint160(addr)), _ADDRESS_LENGTH);
    }

    /**
     * @dev Returns true if the two strings are equal.
     */
    function equal(string memory a, string memory b) internal pure returns (bool) {
        return keccak256(bytes(a)) == keccak256(bytes(b));
    }
}

// File: contracts/game.sol

//SPDX-License-Identifier: MIT
pragma solidity ^0.8.7;


contract BranchClash {
    mapping(address => uint256) public player; //1 is soilder 2 is researcher

    //check to find the node placement, attribute
    mapping(uint256 => mapping(string => inlevel)) public base_struct; 
    mapping(string => bool) private node_achieve;  //Is there a level

    mapping(uint256 => uint256) private num_nodes; //counters how many levels in the layer
    uint256 private sum_layer;

    //tree trunk
    mapping(string => mapping (address => bool)) private edit_modle; 
    mapping (address => string) private map_edit_or; //original level
    mapping (address => string) public map_edit_pr; //present level
    mapping (address => uint256) private map_money;
    mapping (address => uint256) private map_prow;
    mapping (address => uint256) private map_proi;
    mapping (address => uint256) private map_proe;
    mapping (address => uint256) private or_layer;

    mapping(string => mapping (uint256 => string)) public map_map;  //map datas

    uint256 public home_health;
    uint256 public total_attack;
    uint256 public total_monster_blood;
    uint256 public lyr_pr;

    string private main_node;
    //string public main_index;
    string[] public serve_check;
    string[] public serve_check_sec;
    
    string initial_map = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
    uint256[] range1_string_wood = [2,0,2,1,2,3,3,2,1,1,2,3,3,2,1,2,0,2,0,3,0,3,2,0,0,0,0,2,2,0,0,0,0,2,3,0,3,0,3,0,3,3,0,5,5,0,3,3,0,5,5,0,3,3,0,3,0,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,0,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,0,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,0,3,0,5,5,0,3,3,0,5,5,0,3,3,0,5,5,0,3,0,2,0,0,0,0,2,2,0,0,0,0,2,2,0,0,0,0,2,0,1,2,3,3,2,1,1,2,3,3,2,1,1,2,3,3,2,1,0];
    uint256[] range1_string_elec = [1,0,3,2,3,4,3,2,2,2,3,4,3,2,3,1,0,2,0,2,0,5,2,0,0,0,0,4,2,0,0,0,0,5,2,0,3,0,2,0,6,3,0,8,4,0,6,3,0,8,4,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,3,0,8,4,0,6,3,0,8,4,0,6,3,0,8,4,0,3,0,2,0,0,0,0,4,2,0,0,0,0,4,2,0,0,0,0,2,0,2,3,4,3,2,2,2,3,4,3,2,2,2,3,4,3,2,1,0];
    uint256[] range2_string_elec = [2,0,6,7,6,6,6,6,6,6,6,6,6,7,6,4,0,7,0,3,0,8,8,0,0,0,0,8,7,0,0,0,0,8,6,0,9,0,4,0,10,10,0,10,9,0,10,9,0,10,9,0,10,8,0,10,0,4,0,11,11,0,12,11,0,12,11,0,12,11,0,11,9,0,10,0,4,0,10,9,0,10,9,0,10,9,0,10,9,0,10,9,0,10,0,6,0,12,11,0,12,11,0,12,11,0,12,11,0,12,11,0,10,0,5,0,10,9,0,10,9,0,10,9,0,10,9,0,10,9,0,8,0,4,0,0,0,0,8,7,0,0,0,0,8,7,0,0,0,0,6,0,4,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4,4,0];
    uint256[] range3_string_elec = [9,0,9,12,12,8,11,12,10,12,11,8,12,12,9,7,0,9,0,12,0,11,14,0,0,0,0,12,14,0,0,0,0,11,9,0,11,0,15,0,13,17,0,12,16,0,14,17,0,12,17,0,13,11,0,13,0,18,0,15,20,0,14,19,0,16,20,0,14,20,0,15,13,0,15,0,22,0,18,24,0,20,25,0,20,25,0,20,25,0,18,16,0,16,0,20,0,14,19,0,16,20,0,14,19,0,16,20,0,14,13,0,14,0,17,0,12,16,0,14,17,0,12,16,0,14,17,0,12,11,0,12,0,14,0,0,0,0,12,14,0,0,0,0,12,14,0,0,0,0,10,0,12,8,8,11,12,10,12,11,8,11,12,10,12,11,8,8,10,8,0];
    uint256[] wood_dps_rate = [50, 45, 40, 30, 15];
    uint256[] iron_dps_rate = [55,55,55];
    uint256[] elec_dps_rate =  [25,25,75,75,75,125];

    
    uint256 private de_wood_attack;
    uint256 private pro_wood_attack;
    uint256 private de_iron_attack;
    uint256 private pro_iron_attack;
    uint256 private de_elec_attack;
    uint256 private pro_elec_attack;

    uint256 private recent_numnode;
    struct inlevel{
        uint256 timestamp;
        address owner;
        uint256 blood;
        uint256 money;
        string map;
        uint256 wood_protect;
        uint256 iron_protect;
        uint256 elec_protect;
        string block_position; 
        string original_position;
    }

    mapping(string => in_tower) private basic_tower;
    
    struct in_tower{
        string _type;

        uint256 dps;

        uint256 basic_price;
        uint256 merge_price;
        uint256 pro_price;
        }

    mapping(uint256 => in_monster) private basic_monster;
    struct in_monster{
        uint256 m_blood; 
        uint256 m_number;
        uint256 m_interval;
        }
    //tree root
    mapping(uint256 => mapping(string => sec_inlevel)) private sec_struct;
    mapping(string => bool) private sec_node_achieve;  
    mapping(uint256 => uint256) private sec_num_nodes; 
    uint256 public sec_sum_layer;
    mapping(string => mapping (address => bool)) private sec_edit_modle; 
    mapping (address => string) private secmap_edit_or; 
    mapping (address => string) private secmap_edit_pr; 
    mapping (address => uint256) map_orign;
    uint256 private sec_recent_numnode;
    struct sec_inlevel{
            address owner;
            uint256 wood_debuff;
            uint256 iron_debuff;
            uint256 elec_debuff;
            string block_position;
            string original_position; 
        }

    constructor() {
        for (uint256 i=1; i <= 19*9; i++){
            map_map["(0,1)"][i] = "xx";
        }
        map_map["(0,1)"][2] = "H";  map_map["(0,1)"][17] = "R"; map_map["(0,1)"][19] = "/nn";
        map_map["(0,1)"][21] = "R"; map_map["(0,1)"][24] = "R"; map_map["(0,1)"][25] = "R"; map_map["(0,1)"][26] = "R"; map_map["(0,1)"][27] = "R"; map_map["(0,1)"][30] = "R";map_map["(0,1)"][31] = "R";map_map["(0,1)"][32] = "R";map_map["(0,1)"][33] = "R"; map_map["(0,1)"][36] = "R"; map_map["(0,1)"][38] = "/nn";
        map_map["(0,1)"][40] = "R"; map_map["(0,1)"][43] = "R"; map_map["(0,1)"][46] = "R"; map_map["(0,1)"][49] = "R"; map_map["(0,1)"][52] = "R"; map_map["(0,1)"][55] = "R"; map_map["(0,1)"][57] = "/nn";
        map_map["(0,1)"][59] = "R"; map_map["(0,1)"][62] = "R"; map_map["(0,1)"][65] = "R"; map_map["(0,1)"][68] = "R"; map_map["(0,1)"][71] = "R"; map_map["(0,1)"][74] = "R"; map_map["(0,1)"][76] = "/nn";
        map_map["(0,1)"][78] = "R"; map_map["(0,1)"][81] = "R"; map_map["(0,1)"][84] = "R"; map_map["(0,1)"][87] = "R"; map_map["(0,1)"][90] = "R"; map_map["(0,1)"][93] = "R"; map_map["(0,1)"][95] = "/nn";
        map_map["(0,1)"][97] = "R"; map_map["(0,1)"][100] = "R"; map_map["(0,1)"][103] = "R"; map_map["(0,1)"][106] = "R"; map_map["(0,1)"][109] = "R"; map_map["(0,1)"][112] = "R"; map_map["(0,1)"][114] = "/nn";
        map_map["(0,1)"][116] = "R"; map_map["(0,1)"][119] = "R"; map_map["(0,1)"][122] = "R"; map_map["(0,1)"][125] = "R"; map_map["(0,1)"][128] = "R"; map_map["(0,1)"][131] = "R"; map_map["(0,1)"][133] = "/nn";
        map_map["(0,1)"][135] = "R"; map_map["(0,1)"][136] = "R"; map_map["(0,1)"][137] = "R"; map_map["(0,1)"][138] = "R"; map_map["(0,1)"][141] = "R"; map_map["(0,1)"][142] = "R"; map_map["(0,1)"][143] = "R"; map_map["(0,1)"][144] = "R"; map_map["(0,1)"][147] = "R"; map_map["(0,1)"][148] = "R"; map_map["(0,1)"][149] = "R"; map_map["(0,1)"][150] = "R"; map_map["(0,1)"][152] = "/nn";
        map_map["(0,1)"][171] = "/nn";

        base_struct[0]["(0,1)"] = inlevel({
            timestamp: block.timestamp,
            owner: address(this),  
            blood: 10000, 
            money: 1500,
            map: initial_map,
            wood_protect: 0,
            iron_protect: 0,
            elec_protect: 0,
            block_position: "(0,1)",
            original_position: "null"}
            );

        node_achieve["(0,1)"] = true;
        num_nodes[0] = 1; 

        basic_tower["wood"] = in_tower({
            _type: "wood",

            dps: 20,

            basic_price: 100,
            merge_price: 40,
            pro_price: 290
       });
        basic_tower["iron"] = in_tower({
            _type: "iron",

            dps: 30,

            basic_price: 300,
            merge_price: 60,
            pro_price: 290
       });
        basic_tower["elec"] = in_tower({
            _type: "elec",

            dps: 40,

            basic_price: 600,
            merge_price: 100,
            pro_price: 290
       });

        basic_monster[0] = in_monster({
           m_blood: 70,
           m_number: 3,
           m_interval: 3
       });
        sec_struct[0]["(0,1)"] = sec_inlevel({
            owner: address(this), 
            wood_debuff: 0, 
            iron_debuff: 0, 
            elec_debuff: 0,
            block_position: "(0,1)",
            original_position: "null"}
            );
        sec_node_achieve["(0,1)"] = true;
        sec_num_nodes[0] = 1; 
    }
    
    function choose_duty(uint256 d_array) public{//1 soilder 2 researcher
        require(player[msg.sender] == 0, "you have duty");
        require(d_array == 1 || d_array == 2,"choose right duty");
        player[msg.sender] = d_array;
    }

    function in_edit(uint256 c_lyr, uint256 c_idx) public{
        require(player[msg.sender] == 1, "wrong duty");
        string memory c_pos = string(abi.encodePacked("(", Strings.toString(c_lyr),",", Strings.toString(c_idx),")"));
        require (edit_modle[map_edit_pr[msg.sender]][msg.sender] == false,"finish the edit");
        require(node_achieve[c_pos] == true, "null level"); 
        require(base_struct[c_lyr][c_pos].blood > 0,"the level is failed");

        or_layer[msg.sender] = c_lyr;
        if (num_nodes[c_lyr+1] <1){ 
            num_nodes[c_lyr+1] = 1;
        }else if(num_nodes[c_lyr+1] >= 1){ 
            recent_numnode = num_nodes[c_lyr+1] + 1;  
            num_nodes[c_lyr+1] = recent_numnode;  //referesh
        }
        string memory new_pos = string(abi.encodePacked("(", Strings.toString(c_lyr+1),",", Strings.toString(num_nodes[c_lyr+1]),")")); //add index

        for (uint256 i=1; i <= 19*9; i++){ //inherit
            map_map[new_pos][i] = map_map[c_pos][i];
        }
        edit_modle[new_pos][msg.sender] = true;
        map_edit_or[msg.sender] = c_pos;
        map_edit_pr[msg.sender] = new_pos; 

        map_prow[msg.sender] = base_struct[c_lyr][c_pos].wood_protect; 
        map_proi[msg.sender] = base_struct[c_lyr][c_pos].iron_protect;
        map_proe[msg.sender] = base_struct[c_lyr][c_pos].elec_protect;
        map_money[msg.sender] = base_struct[c_lyr][c_pos].money;

        basic_monster[c_lyr+1].m_blood = basic_monster[c_lyr].m_blood +70;
        basic_monster[c_lyr+1].m_number = basic_monster[c_lyr].m_number +1;
        basic_monster[c_lyr+1].m_interval = 3;
    }

    function edit_addtower(uint256 map_tower, string memory tower) public {
        require(player[msg.sender] == 1, "wrong duty");
        string memory new_pos = map_edit_pr[msg.sender];
        require(edit_modle[new_pos][msg.sender], "edit first");
        require(bytes(map_map[new_pos][map_tower]).length == bytes("00").length, "not right position, no xx here"); 
        
        bytes32 towerHash = keccak256(abi.encodePacked(tower));

        require(
            towerHash == keccak256(abi.encodePacked("wood")) ||
            towerHash == keccak256(abi.encodePacked("iron")) ||
            towerHash == keccak256(abi.encodePacked("elec")) ||
            towerHash == keccak256(abi.encodePacked("prow")) ||
            towerHash == keccak256(abi.encodePacked("proi")) ||
            towerHash == keccak256(abi.encodePacked("proe")),
            "not right tower name"
        );

        uint256 _price;

        if (towerHash == keccak256(abi.encodePacked("wood"))) {
            _price = basic_tower["wood"].basic_price;
        } else if (towerHash == keccak256(abi.encodePacked("iron"))) {
            _price = basic_tower["iron"].basic_price;
        } else if (towerHash == keccak256(abi.encodePacked("elec"))) {
            require(bytes(map_map[new_pos][map_tower + 1]).length == bytes("00").length, "not right position, ele tower need two positions");
            _price = basic_tower["elec"].basic_price;
            map_map[new_pos][map_tower + 1] = string(abi.encodePacked(tower, "1"));
        } else if (towerHash == keccak256(abi.encodePacked("prow"))) {
            _price = basic_tower["wood"].pro_price;
            map_prow[msg.sender] += basic_tower["wood"].dps / 2;
        } else if (towerHash == keccak256(abi.encodePacked("proi"))) {
            _price = basic_tower["iron"].pro_price;
            map_proi[msg.sender] += basic_tower["iron"].dps / 2;
        } else if (towerHash == keccak256(abi.encodePacked("proe"))) {
            _price = basic_tower["elec"].pro_price;
            map_proe[msg.sender] += basic_tower["elec"].dps / 2;
        }

        require(map_money[msg.sender] >= _price, "not enough money");
        map_money[msg.sender] -= _price;
        map_map[new_pos][map_tower] = string(abi.encodePacked(tower, "1"));
    }



    function edit_mergetower(uint256 map_tower0, uint256 map_tower1) public {
        require(player[msg.sender] == 1, "wrong duty");
        string memory now_tree_index = map_edit_pr[msg.sender];
        require(edit_modle[now_tree_index][msg.sender], "edit first");

        string memory tower0_str = map_map[now_tree_index][map_tower0];
        string memory tower1_str = map_map[now_tree_index][map_tower1];

        require(bytes(tower0_str).length > 2 && bytes(tower1_str).length > 2, "no tower or not right place");
        require(keccak256(bytes(tower0_str)) == keccak256(bytes(tower1_str)), "should choose the same tower to merge");

        bytes1 fir_bytes = bytes(tower0_str)[0];
        bytes1 sec_bytes = bytes(tower0_str)[3];
        bytes memory type_str = new bytes(4);
        for (uint256 i = 0; i < 4; i++) {
            type_str[i] = bytes(tower0_str)[i];
        }
        string memory _type = string(type_str);

        bytes memory grade_byt = new bytes(bytes(tower0_str).length - 4);
        for (uint256 i = 4; i < bytes(tower0_str).length; i++) {
            grade_byt[i - 4] = bytes(tower0_str)[i];
        }
        string memory _grade_str = string(grade_byt);
        uint256 _grade = stringToUint(_grade_str);

        uint256 _price;

        if (fir_bytes == bytes1("w")) {
            _price = basic_tower["wood"].merge_price;
        } else if (fir_bytes == bytes1("i")) {
            _price = basic_tower["iron"].merge_price;
        } else if (fir_bytes == bytes1("e")) {
            _price = basic_tower["elec"].merge_price;
        }
        else if (sec_bytes == bytes1("w")){
            map_prow[msg.sender] += basic_tower["wood"].dps / 2;
        }else if (sec_bytes == bytes1("i")){
            map_prow[msg.sender] += basic_tower["iron"].dps / 2;
        }else if (sec_bytes == bytes1("e")){
            map_prow[msg.sender] += basic_tower["elec"].dps / 2;
        }
        
        if (fir_bytes == bytes1("w") || fir_bytes == bytes1("i") || fir_bytes == bytes1("e")) {
            require(map_money[msg.sender] >= _price, "no enough money");
            map_money[msg.sender] -= _price;
        }

        if (fir_bytes == bytes1("e")) {
            require(
                keccak256(bytes(map_map[now_tree_index][map_tower0])) == keccak256(bytes(map_map[now_tree_index][map_tower0 + 1])),
                "should choose the righter position of each ele tower to merge"
            );
            require(
                keccak256(bytes(map_map[now_tree_index][map_tower1])) == keccak256(bytes(map_map[now_tree_index][map_tower1 + 1])),
                "should choose the righter position of each ele tower to merge"
            );
            map_map[now_tree_index][map_tower1 + 1] = string(abi.encodePacked(_type, Strings.toString(_grade + 1)));
            map_map[now_tree_index][map_tower0 + 1] = "xx";
        }

        map_map[now_tree_index][map_tower1] = string(abi.encodePacked(_type, Strings.toString(_grade + 1)));
        map_map[now_tree_index][map_tower0] = "xx";
    }

    function sec_in_edit (uint256 choose_layer, uint256 choose_order) public{
        require(player[msg.sender] == 2, "wrong duty");
        string memory position_index = string(abi.encodePacked("(", Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        require (sec_edit_modle[secmap_edit_pr[msg.sender]][msg.sender] == false,"finish the edit"); 
        require(sec_node_achieve[position_index] == true, "null node"); 

        if (sec_num_nodes[choose_layer+1] <1){ 
            sec_num_nodes[choose_layer+1] = 1;
        }else if(sec_num_nodes[choose_layer+1] >= 1){ 
            sec_recent_numnode = sec_num_nodes[choose_layer+1] + 1;  
            sec_num_nodes[choose_layer+1] = sec_recent_numnode;  
        }
        string memory recent_position_index = string(abi.encodePacked("(", Strings.toString(choose_layer+1),",", Strings.toString(sec_num_nodes[choose_layer+1]),")")); //新增index

        map_orign[msg.sender] = choose_layer;
        sec_edit_modle[recent_position_index][msg.sender] = true;
        secmap_edit_or[msg.sender] = position_index; 
        secmap_edit_pr[msg.sender] = recent_position_index; 
    }

    function sec_submitt (uint256 weapontype) public {
        require(player[msg.sender] == 2, "wrong duty");
        require(sec_edit_modle[secmap_edit_pr[msg.sender]][msg.sender] == true, "edit first");
        if (weapontype ==1){
            uint256 num_debuff = sec_struct[map_orign[msg.sender]][secmap_edit_or[msg.sender]].wood_debuff;
            sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]] = sec_struct[map_orign[msg.sender]][secmap_edit_or[msg.sender]];
            sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]].wood_debuff = num_debuff+basic_tower["wood"].dps/2;
        }else if (weapontype ==2){
            uint256 num_debuff = sec_struct[map_orign[msg.sender]][secmap_edit_or[msg.sender]].iron_debuff;
            sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]] = sec_struct[map_orign[msg.sender]][secmap_edit_or[msg.sender]];
            sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]].iron_debuff = num_debuff+basic_tower["iron"].dps/2;
        }else if (weapontype ==3){
            uint256 num_debuff = sec_struct[map_orign[msg.sender]][secmap_edit_or[msg.sender]].elec_debuff;
            sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]] = sec_struct[map_orign[msg.sender]][secmap_edit_or[msg.sender]];
            sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]].elec_debuff = num_debuff+basic_tower["elec"].dps/2;
        }
        sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]].owner = msg.sender;
        sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]].block_position = secmap_edit_pr[msg.sender];
        sec_struct[map_orign[msg.sender]+1][secmap_edit_pr[msg.sender]].original_position = secmap_edit_or[msg.sender];

        sec_edit_modle[secmap_edit_pr[msg.sender]][msg.sender] =false;
        sec_node_achieve[secmap_edit_pr[msg.sender]] = true; 

        if (map_orign[msg.sender]+1> sec_sum_layer){
            sec_sum_layer = map_orign[msg.sender]+1;
        }

        serve_check_sec.push(secmap_edit_pr[msg.sender]);

        secmap_edit_or[msg.sender] = "finish"; 
        secmap_edit_pr[msg.sender] = "finish"; 
    }
    function sec_judg_mainnode (uint256 choose_layer) public view returns(string memory _main_node){
        string memory main_index = string(abi.encodePacked("(", Strings.toString(sec_sum_layer),",1)")); 
        for (uint256 i=sec_sum_layer; i> choose_layer; i--){ 
            main_index = sec_struct[i][main_index].original_position;
        }
        return(main_index);
    }

    function calculateDPS(uint256 _level,uint256[] memory upgrade_ary, uint256 dps) private pure returns (uint256) {
        if(_level == 1){
            return dps;
        }
        for (uint256 i = 1; i < _level; i++) {
            uint256 index = (i - 1) % upgrade_ary.length;
            dps = (dps * 2 + upgrade_ary[index])  ;
        }
        return dps;
    }
    function _compare(uint256 minuend,uint256 subtrahend) private pure returns (uint256) {
        if(minuend>subtrahend){
            return(minuend-subtrahend);
        }
        else{
            return 0;
        }
    }
    function stringToUint(string memory s) private pure returns (uint) {
        bytes memory b = bytes(s);
        uint result = 0;
        for (uint256 i = 0; i < b.length; i++) {
            uint256 c = uint256(uint8(b[i]));
            if (c >= 48 && c <= 57) {
                result = result * 10 + (c - 48);
            }
        }
        return result;
    }
    
    function submit() public {
        require(player[msg.sender] == 1, "wrong duty");
        string memory recent_position_index = map_edit_pr[msg.sender];
        require(edit_modle[recent_position_index][msg.sender] == true, "edit first");
        string memory position_index = map_edit_or[msg.sender];
        string memory this_map = "";
        total_attack = 0;
        uint256 c_lyr = or_layer[msg.sender];
        home_health = base_struct[c_lyr][position_index].blood;
        lyr_pr = c_lyr+1;
        
        for (uint256 i=1; i <= 19*9; i++){
            string memory now_tower =  map_map[recent_position_index][i];
            this_map = string(abi.encodePacked(this_map,",", now_tower));
        }
        
        bool canNotCal = false;
        uint256 _range;
        uint256 _dps;
        main_node = sec_judg_mainnode(lyr_pr);

        de_wood_attack = sec_struct[lyr_pr][main_node].wood_debuff;
        pro_wood_attack = map_prow[msg.sender];
        de_wood_attack = _compare(de_wood_attack,pro_wood_attack);

        de_iron_attack = sec_struct[lyr_pr][main_node].iron_debuff;
        pro_iron_attack = map_proi[msg.sender];
        de_iron_attack = _compare(de_iron_attack,pro_iron_attack);

        de_elec_attack = sec_struct[lyr_pr][main_node].elec_debuff;
        pro_elec_attack = map_proe[msg.sender];
        de_elec_attack = _compare(de_elec_attack,pro_elec_attack);
    
        for (uint256 i=1; i <= 19*9; i++){
            bytes memory bytesStr = bytes(map_map[recent_position_index][i]);
            if(bytesStr.length >= 5){
                bytes1 first_byte_tower = bytesStr[0];
             //get type
            bytes memory grade_byt = new bytes(bytesStr.length - 4);
            for (uint256 j = 4; j < bytesStr.length; j++) {
                grade_byt[j - 4] = bytesStr[j];
            }        
            string memory _grade_str = string(grade_byt);
            uint256 grade = stringToUint(_grade_str);

            //wood
            if(first_byte_tower == bytes1("w")){
                _range = range1_string_wood[i];
                _dps = calculateDPS(grade,wood_dps_rate,basic_tower["wood"].dps); 
                _dps = _compare(_dps,de_wood_attack);
                total_attack += _dps * (_range+(basic_monster[lyr_pr].m_number-1)*basic_monster[lyr_pr].m_interval);
            }
            //Tower "iron"
            else if(first_byte_tower == bytes1("i")){
                _dps = calculateDPS(grade,iron_dps_rate,basic_tower["iron"].dps);
                _dps = _compare(_dps,de_iron_attack);
                total_attack += (10)* _dps;
            }
            //Tower "elec"
            else if(first_byte_tower == bytes1("e")&& canNotCal == false){
                canNotCal = true;
                _dps = calculateDPS(grade,elec_dps_rate,basic_tower["elec"].dps);
                _dps = _compare(_dps,de_elec_attack);
                if(grade>0&&grade<5){
                    _range = range1_string_elec[i];
                }
                else if(grade>=5&&grade<10){ 
                    _range = range2_string_elec[i];
                }
                else{ 
                    _range = range3_string_elec[i];
                }
                total_attack+=_dps*(_range/2+(basic_monster[lyr_pr].m_number-1)*basic_monster[lyr_pr].m_interval);
            }
        else if(first_byte_tower == bytes1("e")&& canNotCal == true){ 
                canNotCal = false; 
                }
            }
        }
        total_monster_blood = basic_monster[lyr_pr].m_blood*basic_monster[lyr_pr].m_number;
        if(total_monster_blood >total_attack && home_health > total_monster_blood-total_attack ){ 
            home_health = home_health + total_attack- total_monster_blood ;
            map_money[msg.sender] = map_money[msg.sender] + 500;
        }
        else if(total_monster_blood >total_attack && home_health < total_monster_blood-total_attack){
            home_health = 0;
        }else{
            map_money[msg.sender] = map_money[msg.sender] + 500;
        }

        base_struct[lyr_pr][recent_position_index] = inlevel({
            timestamp: block.timestamp,
            owner: msg.sender, 
            blood: home_health, 
            money: map_money[msg.sender],
            map: this_map,
            wood_protect: pro_wood_attack,
            iron_protect: pro_iron_attack,
            elec_protect: pro_elec_attack,
            block_position: recent_position_index,
            original_position: position_index
            });

        edit_modle[recent_position_index][msg.sender] = false;
        node_achieve[recent_position_index] = true;

        if (lyr_pr> sum_layer){
            sum_layer = lyr_pr;
        }
        serve_check.push(recent_position_index);

        or_layer[msg.sender] = 0;
        map_edit_pr[msg.sender] = "finish";
        map_edit_or[msg.sender] = "finish";

    }

    function check_map(address wallet) public view returns(string memory thisnode){
        string memory position_index = map_edit_pr[wallet];
        string memory this_map = "";
        for (uint256 i=1; i <= 19*9; i++){
            string memory now_tower =  map_map[position_index][i];
            this_map = string(abi.encodePacked(this_map,",", now_tower));
        }
        return (this_map);
    }

    function check_duty(address wallet) public view returns(uint256){
        return player[wallet];
    }

    function check_time() public view returns(uint256){
        return block.timestamp;
    }

    //check tree trunk level
    function level_time(uint256 choose_layer, uint256 choose_order) public view returns(uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].timestamp);
    }
    function level_blood(uint256 choose_layer, uint256 choose_order) public view returns(uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].blood);
    }
    function level_money(uint256 choose_layer, uint256 choose_order) public view returns(uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].money);
    }

    function level_map(uint256 choose_layer, uint256 choose_order) public view returns(string memory){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].map);
    }
    function level_wodp(uint256 choose_layer, uint256 choose_order) public view returns(uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].wood_protect);
    }
    function level_irop(uint256 choose_layer, uint256 choose_order) public view returns(uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].iron_protect);
    }
    function level_elep(uint256 choose_layer, uint256 choose_order) public view returns(uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].elec_protect);
    }
    function level_bp(uint256 choose_layer, uint256 choose_order) public view returns(string memory){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].block_position);
    }
    function level_op(uint256 choose_layer, uint256 choose_order) public view returns(string memory){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].original_position);
    }

    //check tree root level
    function level_sec_wodd(uint256 choose_layer, uint256 choose_order) public view returns (uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (sec_struct[choose_layer][position_index].wood_debuff);
    }
    function level_sec_irod(uint256 choose_layer, uint256 choose_order) public view returns (uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (sec_struct[choose_layer][position_index].iron_debuff);
    }
    function level_sec_eled(uint256 choose_layer, uint256 choose_order) public view returns (uint256){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (sec_struct[choose_layer][position_index].elec_debuff);
    }
    function level_sec_bp(uint256 choose_layer, uint256 choose_order) public view returns (string memory){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (sec_struct[choose_layer][position_index].block_position);
    }
    function level_sec_op(uint256 choose_layer, uint256 choose_order) public view returns (string memory){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (sec_struct[choose_layer][position_index].original_position);
    }

    function check_num_layer() public view returns(uint256 layer_num){
        return(sum_layer);
    }

    function check_num_nodes(uint256 choose_layer) public view returns (uint256 node_num) {
        return (num_nodes[choose_layer]);
    }

    function sec_check_num_layer() public view returns(uint256 layer_num){
        return(sec_sum_layer);
    }

    function sec_check_num_nodes(uint256 choose_layer) public view returns (uint256 node_num) {
        return (sec_num_nodes[choose_layer]);
    }

    function check_serve() public view returns(uint256){
        return(serve_check.length);
    }

    function serve_level(uint256 num) public view returns(string memory){
        return(serve_check[num]);
    }

    function check_serve_sec() public view returns(uint256){
        return(serve_check_sec.length);
    }

    function serve_level_sec(uint256 num) public view returns(string memory){
        return(serve_check_sec[num]);
    }

    // in edit information
    function check_money(address wallet) public view returns(uint256){
        return(map_money[wallet]);
    }
    function check_level_or(address wallet) public view returns(string memory){
        return(map_edit_or[wallet]);
    }
    function check_level_pr(address wallet) public view returns(string memory){
        return(map_edit_pr[wallet]);
    }

    function seccheck_level_or(address wallet) public view returns (string memory){
        return(secmap_edit_or[wallet]);
    }

    function seccheck_level_pr(address wallet) public view returns (string memory){
        return(secmap_edit_pr[wallet]);
    }

    function level_owner(uint256 choose_layer, uint256 choose_order) public view returns (address){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (base_struct[choose_layer][position_index].owner);
    }

    function level_sec_owner(uint256 choose_layer, uint256 choose_order) public view returns (address){
        string memory position_index = string(abi.encodePacked("(",Strings.toString(choose_layer),",", Strings.toString(choose_order),")"));
        return (sec_struct[choose_layer][position_index].owner);
    }
}