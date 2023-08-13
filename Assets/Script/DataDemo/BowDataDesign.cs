// using System;
// using System.Collections.Generic;
// using System.Text;
// using UnityEngine;
// using Nethereum.Util;
//
// public class BowDataDesign : MonoBehaviour
// {
//     // Define your C# variables based on the Solidity structures
//     private Dictionary<uint, Dictionary<string, Inlevel>> base_struct = new Dictionary<uint, Dictionary<string, Inlevel>>();
//     private Dictionary<string, bool> node_achieve = new Dictionary<string, bool>();
//
//     private Dictionary<uint, uint> num_nodes = new Dictionary<uint, uint>();
//     private uint sum_layer;
//
//     private Dictionary<string, Dictionary<string, bool>> edit_modle = new Dictionary<string, Dictionary<string, bool>>();
//     private Dictionary<string, string> map_edit_or = new Dictionary<string, string>();
//     private Dictionary<string, string> map_edit_pr = new Dictionary<string, string>();
//     private Dictionary<string, uint> map_money = new Dictionary<string, uint>();
//     private Dictionary<string, uint> map_prow = new Dictionary<string, uint>();
//     private Dictionary<string, uint> map_proi = new Dictionary<string, uint>();
//     private Dictionary<string, uint> map_proe = new Dictionary<string, uint>();
//
//     public string addressThis;
//
//     private Dictionary<string, Dictionary<uint, string>> map_map = new Dictionary<string, Dictionary<uint, string>>();
//
//     public uint home_health;
//     public uint total_attack;
//
//     private string main_node;
//     // public string main_index;
//     public List<string> node_list = new List<string>();
//     public List<string> sec_node_list = new List<string>();
//
//     public List<string> serve_check = new List<string>();
//     public List<string> serve_check_sec = new List<string>();
//
//     private string initial_map = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
//     private string range1_string_wood = "2,0,2,1,2,3,3,2,1,1,2,3,3,2,1,2,0,2,0,3,0,3,2,0,0,0,0,2,2,0,0,0,0,2,3,0,3,0,3,0,3,3,0,5,5,0,3,3,0,5,5,0,3,3,0,3,0,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,0,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,3,0,3,0,3,0,5,5,0,3,3,0,5,5,0,3,3,0,5,5,0,3,0,2,0,0,0,0,2,2,0,0,0,0,2,2,0,0,0,0,2,0,1,2,3,3,2,1,1,2,3,3,2,1,1,2,3,3,2,1,0,/";
//     private string range1_string_elec = "1,0,3,2,3,4,3,2,2,2,3,4,3,2,3,1,0,2,0,2,0,5,2,0,0,0,0,4,2,0,0,0,0,5,2,0,3,0,2,0,6,3,0,8,4,0,6,3,0,8,4,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,3,0,8,4,0,6,3,0,8,4,0,6,3,0,8,4,0,3,0,2,0,0,0,0,4,2,0,0,0,0,4,2,0,0,0,0,2,0,2,3,4,3,2,2,2,3,4,3,2,2,2,3,4,3,2,1,0,/";
//     private string range2_string_elec = "2,0,6,7,6,6,6,6,6,6,6,6,6,7,6,4,0,7,0,3,0,8,8,0,0,0,0,8,7,0,0,0,0,8,6,0,9,0,4,0,10,10,0,10,9,0,10,9,0,10,9,0,10,8,0,10,0,4,0,11,11,0,12,11,0,12,11,0,12,11,0,11,9,0,10,0,4,0,10,9,0,10,9,0,10,9,0,10,9,0,10,9,0,10,0,6,0,12,11,0,12,11,0,12,11,0,12,11,0,12,11,0,10,0,5,0,10,9,0,10,9,0,10,9,0,10,9,0,10,9,0,8,0,4,0,0,0,0,8,7,0,0,0,0,8,7,0,0,0,0,6,0,4,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4,4,0,/";
//     private string range3_string_elec = "9,0,9,12,12,8,11,12,10,12,11,8,12,12,9,7,0,9,0,12,0,11,14,0,0,0,0,12,14,0,0,0,0,11,9,0,11,0,15,0,13,17,0,12,16,0,14,17,0,12,17,0,13,11,0,13,0,18,0,15,20,0,14,19,0,16,20,0,14,20,0,15,13,0,15,0,22,0,18,24,0,20,25,0,20,25,0,20,25,0,18,16,0,16,0,20,0,14,19,0,16,20,0,14,19,0,16,20,0,14,13,0,14,0,17,0,12,16,0,14,17,0,12,16,0,14,17,0,12,11,0,12,0,14,0,0,0,0,12,14,0,0,0,0,12,14,0,0,0,0,10,0,12,8,8,11,12,10,12,11,8,11,12,10,12,11,8,8,10,8,0,/";
//
//     private uint de_wood_attack;
//     private uint pro_wood_attack;
//     private uint de_iron_attack;
//     private uint pro_iron_attack;
//     private uint de_elec_attack;
//     private uint pro_elec_attack;
//
//     private uint recent_numnode;
//     public struct Inlevel
//     {
//         public uint timestamp;
//         public string owner;
//         public uint blood;
//         public uint money;
//         public string map;
//         public uint wood_protect;
//         public uint iron_protect;
//         public uint elec_protect;
//         public string block_position;
//         public string original_position;
//     }
//
//     private Dictionary<string, InTower> basic_tower = new Dictionary<string, InTower>();
//
//     public struct InTower
//     {
//         public string _type;
//
//         public uint b_attack;
//         public uint b_speed;
//
//         public uint grade_s_2_a;
//
//         public uint speed_rate;
//         public uint attack_rate;
//
//         public uint basic_price;
//         public uint upgrade_price;
//
//         public uint pro_price;
//     }
//
//     private Dictionary<uint, InMonster> basic_monster = new Dictionary<uint, InMonster>();
//     public struct InMonster
//     {
//         public uint m_speed;
//         public uint m_blood;
//         public uint m_number;
//         public uint m_interval;
//     }
//
//     private Dictionary<uint, Dictionary<string, SecInlevel>> sec_struct = new Dictionary<uint, Dictionary<string, SecInlevel>>();
//     private Dictionary<string, bool> sec_node_achieve = new Dictionary<string, bool>();
//     private Dictionary<uint, uint> sec_num_nodes = new Dictionary<uint, uint>();
//     public uint sec_sum_layer;
//     private Dictionary<string, Dictionary<string, bool>> sec_edit_modle = new Dictionary<string, Dictionary<string, bool>>();
//     private Dictionary<string, string> secmap_edit_or = new Dictionary<string, string>();
//     private Dictionary<string, string> secmap_edit_pr = new Dictionary<string, string>();
//     private Dictionary<string, uint> map_orign = new Dictionary<string, uint>();
//     private uint sec_recent_numnode;
//     public struct SecInlevel
//     {
//         public string owner;
//         public uint wood_debuff;
//         public uint iron_debuff;
//         public uint elec_debuff;
//         public string block_position;
//         public string original_position;
//     }
//         private void Start()
//     {
//         for (uint i = 1; i <= 19 * 9; i++)
//         {
//             map_map["(0,1)"][i] = "xx";
//         }
//         map_map["(0,1)"][2] = "H"; map_map["(0,1)"][17] = "R"; map_map["(0,1)"][19] = "/nn";
//         map_map["(0,1)"][21] = "R"; map_map["(0,1)"][24] = "R"; map_map["(0,1)"][25] = "R"; map_map["(0,1)"][26] = "R"; map_map["(0,1)"][27] = "R"; map_map["(0,1)"][30] = "R"; map_map["(0,1)"][31] = "R"; map_map["(0,1)"][32] = "R"; map_map["(0,1)"][33] = "R"; map_map["(0,1)"][36] = "R"; map_map["(0,1)"][38] = "/nn";
//         map_map["(0,1)"][40] = "R"; map_map["(0,1)"][43] = "R"; map_map["(0,1)"][46] = "R"; map_map["(0,1)"][49] = "R"; map_map["(0,1)"][52] = "R"; map_map["(0,1)"][55] = "R"; map_map["(0,1)"][57] = "/nn";
//         map_map["(0,1)"][59] = "R"; map_map["(0,1)"][62] = "R"; map_map["(0,1)"][65] = "R"; map_map["(0,1)"][68] = "R"; map_map["(0,1)"][71] = "R"; map_map["(0,1)"][74] = "R"; map_map["(0,1)"][76] = "/nn";
//         map_map["(0,1)"][78] = "R"; map_map["(0,1)"][81] = "R"; map_map["(0,1)"][84] = "R"; map_map["(0,1)"][87] = "R"; map_map["(0,1)"][90] = "R"; map_map["(0,1)"][93] = "R"; map_map["(0,1)"][95] = "/nn";
//         map_map["(0,1)"][97] = "R"; map_map["(0,1)"][100] = "R"; map_map["(0,1)"][103] = "R"; map_map["(0,1)"][106] = "R"; map_map["(0,1)"][109] = "R"; map_map["(0,1)"][112] = "R"; map_map["(0,1)"][114] = "/nn";
//         map_map["(0,1)"][116] = "R"; map_map["(0,1)"][119] = "R"; map_map["(0,1)"][122] = "R"; map_map["(0,1)"][125] = "R"; map_map["(0,1)"][128] = "R"; map_map["(0,1)"][131] = "R"; map_map["(0,1)"][133] = "/nn";
//         map_map["(0,1)"][135] = "R"; map_map["(0,1)"][136] = "R"; map_map["(0,1)"][137] = "R"; map_map["(0,1)"][138] = "R"; map_map["(0,1)"][141] = "R"; map_map["(0,1)"][142] = "R"; map_map["(0,1)"][143] = "R"; map_map["(0,1)"][144] = "R"; map_map["(0,1)"][147] = "R"; map_map["(0,1)"][148] = "R"; map_map["(0,1)"][149] = "R"; map_map["(0,1)"][150] = "R"; map_map["(0,1)"][152] = "/nn";
//         map_map["(0,1)"][171] = "/nn";
//
//         node_list.Add("(0,1)");
//         sec_node_list.Add("(0,1)");
//
//         base_struct[0]["(0,1)"] = new Inlevel
//         {
//             timestamp = (uint)Time.time,
//             owner = addressThis, // Replace addressThis with the appropriate value
//             blood = 1000,
//             money = 2000,
//             map = initial_map,
//             wood_protect = 0,
//             iron_protect = 0,
//             elec_protect = 0,
//             block_position = "(0,1)",
//             original_position = "null"
//         };
//
//         node_achieve["(0,1)"] = true;
//         num_nodes[0] = 1;
//
//         basic_tower["wood"] = new InTower
//         {
//             _type = "wood",
//
//             b_attack = 10,
//             b_speed = 4,
//
//             grade_s_2_a = 10,
//
//             speed_rate = 2,
//             attack_rate = 10,
//
//             basic_price = 600,
//             upgrade_price = 200,
//
//             pro_price = 300
//         };
//         basic_tower["iron"] = new InTower
//         {
//             _type = "iron",
//
//             b_attack = 60,
//             b_speed = 1,
//
//             grade_s_2_a = 5,
//
//             speed_rate = 1,
//             attack_rate = 20,
//
//             basic_price = 900,
//             upgrade_price = 400,
//
//             pro_price = 450
//         };
//         basic_tower["elec"] = new InTower
//         {
//             _type = "elec",
//
//             b_attack = 5,
//             b_speed = 10,
//
//             grade_s_2_a = 20,
//
//             speed_rate = 4,
//             attack_rate = 5,
//
//             basic_price = 1200,
//             upgrade_price = 500,
//
//             pro_price = 600
//         };
//         basic_monster[0] = new InMonster
//         {
//             m_speed = 2,
//             m_blood = 100,
//             m_number = 2,
//             m_interval = 3
//         };
//         sec_struct[0]["(0,1)"] = new SecInlevel
//         {
//             owner = addressThis, // Replace addressThis with the appropriate value
//             wood_debuff = 0,
//             iron_debuff = 0,
//             elec_debuff = 0,
//             block_position = "(0,1)",
//             original_position = "null"
//         };
//         sec_node_achieve["(0,1)"] = true;
//         sec_num_nodes[0] = 1;
//     }
//         public void InEdit(uint c_lyr, uint c_idx)
//         {
//             string c_pos = string.Concat("(", c_lyr.ToString(), ",", c_idx.ToString(), ")");
//         
//             if (!edit_modle[map_edit_pr[addressThis]][addressThis])
//             {
//                 throw new Exception("finish the edit");
//             }
//
//             if (!node_achieve.ContainsKey(c_pos) || !node_achieve[c_pos])
//             {
//                 throw new Exception("null node");
//             }
//
//             if (num_nodes[c_lyr + 1] < 1)
//             {
//                 num_nodes[c_lyr + 1] = 1;
//             }
//             else if (num_nodes[c_lyr + 1] >= 1)
//             {
//                 recent_numnode = num_nodes[c_lyr + 1] + 1;
//                 num_nodes[c_lyr + 1] = recent_numnode; // referesh
//             }
//
//             string new_pos = string.Concat("(", (c_lyr + 1).ToString(), ",", num_nodes[c_lyr + 1].ToString(), ")");
//         
//             for (uint i = 1; i <= 19 * 9; i++)
//             {
//                 map_map[new_pos][i] = map_map[c_pos][i];
//             }
//
//             edit_modle[new_pos][addressThis] = true;
//             map_edit_or[addressThis] = c_pos;
//             map_edit_pr[addressThis] = new_pos;
//
//             map_prow[addressThis] = base_struct[c_lyr][c_pos].wood_protect;
//             map_proi[addressThis] = base_struct[c_lyr][c_pos].iron_protect;
//             map_proe[addressThis] = base_struct[c_lyr][c_pos].elec_protect;
//             map_money[addressThis] = base_struct[c_lyr][c_pos].money;
//         }
//     public void EditAddTower(uint map_tower, string tower)
//     {
//         string new_pos = map_edit_pr[addressThis]; // Assuming msg_sender is a variable representing the sender's address
//         if (!edit_modle[new_pos][addressThis])
//         {
//             throw new Exception("edit first");
//         }
//
//         if (map_map[new_pos][map_tower].Length != "00".Length)
//         {
//             throw new Exception("not right position, no 00 here");
//         }
//
//         string towerHash = BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes(tower))).Replace("-", "");
//
//         if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("wood"))).Replace("-", "") ||
//             towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("iron"))).Replace("-", "") ||
//             towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("elec"))).Replace("-", "") ||
//             towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("prow"))).Replace("-", "") ||
//             towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("proi"))).Replace("-", "") ||
//             towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("proe"))).Replace("-", ""))
//         {
//             throw new Exception("not right tower name");
//         }
//
//         uint _price = 0;
//
//         if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("wood"))).Replace("-", ""))
//         {
//             _price = basic_tower["wood"].basic_price;
//         }
//         else if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("iron"))).Replace("-", ""))
//         {
//             _price = basic_tower["iron"].basic_price;
//         }
//         else if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("elec"))).Replace("-", ""))
//         {
//             if (map_map[new_pos][map_tower + 1].Length != "00".Length)
//             {
//                 throw new Exception("not right position, ele tower needs two positions");
//             }
//             _price = basic_tower["elec"].basic_price;
//             map_map[new_pos][map_tower + 1] = string.Concat(tower, "1");
//         }
//         else if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("prow"))).Replace("-", ""))
//         {
//             _price = basic_tower["wood"].pro_price;
//             map_prow[addressThis] += basic_tower["wood"].b_attack * basic_tower["wood"].b_speed / 2;
//         }
//         else if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("proi"))).Replace("-", ""))
//         {
//             _price = basic_tower["iron"].pro_price;
//             map_proi[addressThis] += basic_tower["iron"].b_attack * basic_tower["iron"].b_speed / 2;
//         }
//         else if (towerHash == BitConverter.ToString(Keccak256(Encoding.UTF8.GetBytes("proe"))).Replace("-", ""))
//         {
//             _price = basic_tower["elec"].pro_price;
//             map_proe[addressThis] += basic_tower["elec"].b_attack * basic_tower["elec"].b_speed / 2;
//         }
//
//         if (map_money[addressThis] < _price)
//         {
//             throw new Exception("not enough money");
//         }
//         map_money[addressThis] -= _price;
//         map_map[new_pos][map_tower] = string.Concat(tower, "1");
//     }
//
//     private static byte[] Keccak256(byte[] input)
//     {
//         return new Sha3Keccack().CalculateHash(input);
//     }
//     public void EditMergeTower(uint map_tower0, uint map_tower1)
//     {
//         string now_tree_index = map_edit_pr[addressThis]; // Assuming msg_sender is a variable representing the sender's address
//         if (!edit_modle[now_tree_index][addressThis])
//         {
//             throw new Exception("edit first");
//         }
//
//         string tower0_str = map_map[now_tree_index][map_tower0];
//         string tower1_str = map_map[now_tree_index][map_tower1];
//
//         if (tower0_str.Length <= 2 || tower1_str.Length <= 2)
//         {
//             throw new Exception("no tower or not right place");
//         }
//
//         if (Keccak256(Encoding.UTF8.GetBytes(tower0_str)) != Keccak256(Encoding.UTF8.GetBytes(tower1_str)))
//         {
//             throw new Exception("should choose the same tower to merge");
//         }
//
//         char fir_bytes = tower0_str[0];
//         char sec_bytes = tower0_str[1];
//         string type_str = tower0_str.Substring(0, 4);
//         string _type = type_str;
//
//         string grade_str = tower0_str.Substring(4);
//         uint _grade = BitConverter.ToUInt32(Encoding.UTF8.GetBytes(grade_str), 0);
//
//         uint _price = 0;
//
//         if (fir_bytes == 'w')
//         {
//             _price = basic_tower["wood"].upgrade_price;
//         }
//         else if (fir_bytes == 'i')
//         {
//             _price = basic_tower["iron"].upgrade_price;
//         }
//         else if (fir_bytes == 'e')
//         {
//             _price = basic_tower["elec"].upgrade_price;
//         }
//         else if (sec_bytes == 'w' || sec_bytes == 'i' || sec_bytes == 'e')
//         {
//             _price = basic_tower[_type].pro_price;
//         }
//
//         if (fir_bytes == 'w' || fir_bytes == 'i' || fir_bytes == 'e' || sec_bytes == 'w' || sec_bytes == 'i' || sec_bytes == 'e')
//         {
//             if (map_money[addressThis] < _price)
//             {
//                 throw new Exception("no enough money");
//             }
//
//             map_money[addressThis] -= _price;
//
//             if (sec_bytes == 'w')
//             {
//                 map_prow[addressThis] += basic_tower["wood"].b_attack * basic_tower["wood"].b_speed / 2;
//             }
//             else if (sec_bytes == 'i')
//             {
//                 map_prow[addressThis] += basic_tower["iron"].b_attack * basic_tower["iron"].b_speed / 2;
//             }
//             else if (sec_bytes == 'e')
//             {
//                 map_prow[addressThis] += basic_tower["elec"].b_attack * basic_tower["elec"].b_speed / 2;
//             }
//         }
//
//         if (fir_bytes == 'e')
//         {
//             if (Keccak256(Encoding.UTF8.GetBytes(map_map[now_tree_index][map_tower0])) != Keccak256(Encoding.UTF8.GetBytes(map_map[now_tree_index][map_tower0 + 1])))
//             {
//                 throw new Exception("should choose the righter position of each ele tower to merge");
//             }
//
//             if (Keccak256(Encoding.UTF8.GetBytes(map_map[now_tree_index][map_tower1])) != Keccak256(Encoding.UTF8.GetBytes(map_map[now_tree_index][map_tower1 + 1])))
//             {
//                 throw new Exception("should choose the righter position of each ele tower to merge");
//             }
//
//             map_map[now_tree_index][map_tower1 + 1] = string.Concat(_type, _grade + 1);
//             map_map[now_tree_index][map_tower0 + 1] = "xx";
//         }
//
//         map_map[now_tree_index][map_tower1] = string.Concat(_type, _grade + 1);
//         map_map[now_tree_index][map_tower0] = "xx";
//     }
//     
//     public void Submit(uint choose_layer)
//     {
//         string recent_position_index = map_edit_pr[addressThis];
//         if (edit_modle[recent_position_index][addressThis] != true)
//         {
//             throw new Exception("edit first");
//         }
//
//         string position_index = map_edit_or[addressThis];
//         string this_map = "";
//         total_attack = 0;
//         home_health = base_struct[choose_layer][position_index].blood;
//
//         for (int i = 1; i <= 19 * 9; i++)
//         {
//             string now_tower = map_map[recent_position_index][i];
//             this_map = string.Concat(this_map, ",", now_tower);
//         }
//
//         uint canNotCal = 0;
//         uint _range;
//         uint _attack;
//         uint _speed;
//         uint _time;
//         //main_node = SecJudgMainnode(choose_layer + 1);
//
//         de_wood_attack = sec_struct[choose_layer + 1][main_node].wood_debuff;
//         pro_wood_attack = map_prow[addressThis];
//         if (de_wood_attack > pro_wood_attack)
//         {
//             de_wood_attack = de_wood_attack - pro_wood_attack;
//         }
//         else
//         {
//             de_wood_attack = 0;
//         }
//
//         // ... (Similar blocks for de_iron_attack and de_elec_attack) ...
//
//         for (int i = 1; i <= 19 * 9; i++)
//         {
//             byte[] bytesStr = Encoding.UTF8.GetBytes(map_map[recent_position_index][i]);
//             byte first_byte_tower = bytesStr[0];
//             byte[] grade_string = new byte[bytesStr.Length - 4];
//             for (int j = 4; j < bytesStr.Length; j++)
//             {
//                 grade_string[j - 4] = bytesStr[j];
//             }
//             uint grade = BitConverter.ToUInt32(grade_string, 0);
//
//             // ... (Similar blocks for wood, iron, and elec towers) ...
//
//             else if (first_byte_tower == (byte)'e' && canNotCal == 1)
//             {
//                 canNotCal = 0;
//             }
//         }
//         uint total_monster_blood = basic_monster[0].m_blood * basic_monster[0].m_number;
//         uint home_health = base_struct[choose_layer][position_index].blood;
//         if (total_monster_blood > home_health && home_health > total_monster_blood - total_attack)
//         {
//             home_health = home_health + total_attack - total_monster_blood;
//             map_money[addressThis] = map_money[addressThis] + 1000;
//         }
//         else if (total_monster_blood > home_health && home_health < total_monster_blood - total_attack)
//         {
//             home_health = 0;
//         }
//
//         base_struct[choose_layer + 1][recent_position_index] = new InLevel
//         {
//             timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
//             owner = msg_sender,
//             blood = home_health,
//             money = map_money[msg_sender],
//             map = this_map,
//             wood_protect = pro_wood_attack,
//             iron_protect = pro_iron_attack,
//             elec_protect = pro_elec_attack,
//             block_position = recent_position_index,
//             original_position = position_index
//         };
//
//         edit_modle[recent_position_index][msg_sender] = false;
//         node_achieve[recent_position_index] = true;
//
//         if (choose_layer + 1 > sum_layer)
//         {
//             sum_layer = choose_layer + 1;
//         }
//         node_list.Add(recent_position_index);
//         serve_check.Add(recent_position_index);
//     }
// }
