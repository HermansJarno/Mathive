using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevels : MonoBehaviour {

  static int[] level1 = new int[2] { 4, 4 };
  static int[] level2 = new int[2] { 4, 5 };
  static int[] level3 = new int[2] { 5, 4 };
  static int[] level4 = new int[2] { 6, 3 };
  static int[] level5 = new int[2] { 6, 4 };
  static int[] level6 = new int[2] { 4, 6 };
  static int[] level7 = new int[2] { 3, 7 };
  static int[] level8 = new int[2] { 4, 7 };
  static int[] level9 = new int[2] { 5, 8 };

  int[][] levels = new int[][]
  {
    level1,
    level2,
    level3,
    level4,
    level5,
    level6,
    level7,
    level8,
    level9
  };

  #region Data Setup: Leave all as it is
  // Name --> a Comma between every number
  static int[] LeaveAllAsItis = new int[0] {  };
  #endregion

  #region Data Setup: Leave 1 hive empty
  // Name --> a Comma between every number
  static int[] LeaveIndex1Open = new int[1] { 1 };
  static int[] LeaveIndex2Open = new int[1] { 2 };
  static int[] LeaveIndex3Open = new int[1] { 3 };
  static int[] LeaveIndex4Open = new int[1] { 4 };
  static int[] LeaveIndex5Open = new int[1] { 5 };
  static int[] LeaveIndex6Open = new int[1] { 6 };
  static int[] LeaveIndex7Open = new int[1] { 7 };
  static int[] LeaveIndex8Open = new int[1] { 8 };
  #endregion

  #region Data Setup: Leave 2 hives empty
  static int[] LeaveIndex12Open = new int[2] { 1, 2 };
  static int[] LeaveIndex13Open = new int[2] { 1, 3 };
  static int[] LeaveIndex14Open = new int[2] { 1, 4 };
  static int[] LeaveIndex15Open = new int[2] { 1, 5 };
  static int[] LeaveIndex16Open = new int[2] { 1, 6 };
  static int[] LeaveIndex17Open = new int[2] { 1, 7 };
  static int[] LeaveIndex18Open = new int[2] { 1, 8 };

  static int[] LeaveIndex23Open = new int[2] { 2, 3 };
  static int[] LeaveIndex24Open = new int[2] { 2, 4 };
  static int[] LeaveIndex25Open = new int[2] { 2, 5 };
  static int[] LeaveIndex26Open = new int[2] { 2, 6 };
  static int[] LeaveIndex27Open = new int[2] { 2, 7 };
  static int[] LeaveIndex28Open = new int[2] { 2, 8 };

  static int[] LeaveIndex34Open = new int[2] { 3, 4 };
  static int[] LeaveIndex35Open = new int[2] { 3, 5 };
  static int[] LeaveIndex36Open = new int[2] { 3, 6 };
  static int[] LeaveIndex37Open = new int[2] { 3, 7 };
  static int[] LeaveIndex38Open = new int[2] { 3, 8 };


  static int[] LeaveIndex45Open = new int[2] { 4, 5 };
  static int[] LeaveIndex46Open = new int[2] { 4, 6 };
  static int[] LeaveIndex47Open = new int[2] { 4, 7 };
  static int[] LeaveIndex48Open = new int[2] { 4, 8 };

  static int[] LeaveIndex56Open = new int[2] { 5, 6 };
  static int[] LeaveIndex57Open = new int[2] { 5, 7 };
  static int[] LeaveIndex58Open = new int[2] { 5, 8 };

  static int[] LeaveIndex67Open = new int[2] { 6, 7 };
  static int[] LeaveIndex68Open = new int[2] { 6, 8 };
  static int[] leaveIndex78Open = new int[2] { 7, 8 };
  #endregion


  // Number of records must be the same as the number of rows
  static int[][] level1EmptyHives = new int[][]
  {
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex4Open,
    LeaveIndex12Open
  };
  static int[][] level2EmptyHives = new int[][]
  {
    LeaveIndex5Open,
    LeaveIndex2Open,
    LeaveIndex5Open,
    LeaveIndex12Open
  };
  static int[][] level3EmptyHives = new int[][]
  {
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex3Open,
    LeaveIndex4Open,
    LeaveIndex2Open
  };

  static int[][] level4EmptyHives = new int[][]
{
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveAllAsItis,
    LeaveIndex23Open,
    LeaveAllAsItis,
    LeaveIndex3Open
};

  static int[][] level5EmptyHives = new int[][]
{
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex4Open,
    LeaveIndex12Open
};

  static int[][] level6EmptyHives = new int[][]
{
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex4Open,
    LeaveIndex12Open
};

  static int[][] level7EmptyHives = new int[][]
{
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex4Open,
    LeaveIndex12Open
};

  static int[][] level8EmptyHives = new int[][]
{
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex4Open,
    LeaveIndex12Open
};

  static int[][] level9EmptyHives = new int[][]
{
    LeaveIndex1Open,
    LeaveAllAsItis,
    LeaveIndex4Open,
    LeaveIndex12Open
};

  int[][][] emptyHivesLevels = new int[][][]
  {
    level1EmptyHives,
    level2EmptyHives,
    level3EmptyHives,
    level4EmptyHives,
    level5EmptyHives,
    level6EmptyHives,
    level7EmptyHives,
    level8EmptyHives,
    level9EmptyHives,
  };

  public int[][][] EmptyHivesLevels
  {
    get
    {
      return emptyHivesLevels;
    }
  }

  public int[][] Levels {
    get
    {
      return levels;
    }
  }
}
