using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR
{
    public static class GLV
    {
        #region Assets Enums
        // Enum for the selection Assets
        public enum Assets
        {
            WorkObject,
            HumanTool,
            Robot,
            RobotTool,
            NumberofTypes   // This has to be the last variable since it is used for counting
        }

        // Enum for the selection of Assets Dependencies
        public enum AssetMenuSelector
        {
            WorkObject,
            HumanTool,
            Robot,
            RobotTools,
            Independent
        }
        #endregion

        #region Tags
        public const string WOBehaviourTag = "Work Object Behaviour";
        public const string HTBehaviourTag = "Human Tool Behaviour";
        public const string RobotBehaviourTag = "Robot Behaviour";

        public const string WOTag = "Work Object";
        public const string HTTag = "Human Tool";
        public const string RobotTag = "Robot";
        public const string RobotToolsTag = "Robot Tool";
        public const string VirtualRobotInvisiblePartsTag = "VirtualRobotInvisible";
        public const string RealRobotInvisiblePartsTag = "RealRobotInvisible";

        public const string PalletTag = "Pallet"; // Not implemented
        #endregion

        #region Showcase 2
        public const string BoxDowntag = "BoxDown";
        public const string BoxUptag = "BoxUp";
        public const string Batterytag = "Battery";
        public const string Wiretag = "Wire";
        public const string AssembleDonetag = "AssembleDone";
        #endregion

        #region Showcase 3
        public const string BottomTag = "Bot";
        public const string TopTag = "Top";
        public const string Rod1Tag = "Rod1";
        public const string Rod2Tag = "Rod2";
        public const string ScrewTag = "Screw";

        public const string DinamicParenttag = "DinamicParent";
        public const string ScrewAssambledTag = "ScrewMagnet";
        #endregion

        #region Names
        public const string SparkName = "Spark";
        public const string BrushName = "PaintBrush";
        public const string WeldingPointsName = "WeldingPoints";
        public const string PaintPointsName = "PaintPoints";
        public const string FinalEventName = "Final Event";
        public const string WeldingName = "Welding_1";
        public const string ScrewDriverPoints = "Screw Driver Points";
        public const string TextName = "Text";

        public const string ShowcaseManagerName = "Showcase Manager";
        public const string WorkObjectContainerName = "Work Object Container";
        public const string HumanToolContainerName = "Human Tool Container";
        public const string RobotToolContainerName = "Robot Container";
        public const string ScrewDriverName = "ScrewDriver";
        #endregion

        #region Static Variables
        public static bool PLC { get; set; } = false;
        public static int PLC_MU_index { get; set; } = 0;
        #endregion
    }
}
