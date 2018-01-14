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

  public int[][] Levels {
    get
    {
      return levels;
    }
  }
}
