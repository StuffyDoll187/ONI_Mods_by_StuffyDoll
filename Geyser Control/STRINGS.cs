

namespace Geyser_Control
{
    internal class STRINGS
    {
        public class SLIDERS
        {
            public class GEYSERSLIDERS
            {
                public static LocString NAME = "";
            }
            public class MASSPERCYCLECONTROLLER
            {
                public static LocString NAME = "Mass Per Cycle";
                public static LocString TOOLTIP = "Theoretical mass if always active";
                public static LocString UNITS = "kg";
            }
            public class ITERATIONLENGTHCONTROLLER
            {
                public static LocString NAME = "Iteration Length";
                public static LocString TOOLTIP = "Total Eruption Period including Idle";
                public static LocString UNITS = "s";
            }
            public class ITERATIONPERCENTCONTROLLER
            {
                public static LocString NAME = "Iteration Percent";
                public static LocString TOOLTIP = "Percentage of Total Eruption Period to erupt";
                public static LocString UNITS = "%";
            }
            public class YEARLENGTHCONTROLLER
            {
                public static LocString NAME = "Year Length";
                public static LocString TOOLTIP = "Total Life Cycle including Dormancy";
                public static LocString UNITS = "s";
            }
            public class YEARPERCENTCONTROLLER
            {
                public static LocString NAME = "Year Percent";
                public static LocString TOOLTIP = "Percentage of Total Life Cycle to be Active";
                public static LocString UNITS = "%";
            }
            /*public class MAXPRESSURECONTROLS
            {
                public static LocString NAME = "Overpressure Threshold";
                public static LocString TOOLTIP = "Overpressure Threshold";
                public static LocString UNITS = "kg";
            }*/
        }
        public class BUTTONS
        {
            public class CONFIRM
            {
                public static LocString NAME = "CONFIRM";
            }
            public class SLIDERBUTTON
            {
                public class SHOW
                {
                    public static LocString NAME = "Show Slider Controls";                    
                }
                public class HIDE
                {
                    public static LocString NAME = "Hide Slider Controls";
                    public static LocString TOOLTIP = "";
                }
            }
            public class RESETBUTTON
            {
                public static LocString NAME = "Reset to Original Values";                
                public static LocString TOOLTIP = "";
                                
            }
            public class RANDOMIZESLIDERSBUTTON
            {
                public static LocString NAME = "Randomize Values";
                public static LocString TOOLTIP = "Sets random values for the 5 primary sliders";
            }
            public class DORMANCYBUTTON
            {
                public class ENTER
                {
                    public static LocString NAME = "Enter Dormancy";
                    public static LocString TOOLTIP = "";
                }
                public class EXIT
                {
                    public static LocString NAME = "Exit Dormancy";
                    public static LocString TOOLTIP = "";
                }
            }
            public class ERUPTIONBUTTON
            {
                public static LocString NAME = "Advance To Next Eruption";
                public static LocString TOOLTIP = "";

            }
            public class DISABLED
            {
                public static LocString TOOLTIP = "Analyze To Enable";
            }
        }
        public class CHECKBOX
        {
            public class UNCAPPRESSURE
            {
                public static LocString NAME = "Uncap Pressure";
                public static LocString TOOLTIP = "Sets Overpressure limit to 1e30 kg";
            }
        }
        public class CONFIG
        {
            public class BREAKVANILLALIMITS
            {
                public static LocString TITLE = "Break Vanilla Limits";
                public static LocString TOOLTIP = "Allows values outside normal range";
            }
            public class BREAKFACTOR
            {
                public static LocString TITLE = "Break Limit Factor";
                public static LocString TOOLTIP = "Factor by which to expand Mass Per Cycle slider limits";
            }
            public class RESETBUTTON
            {
                public static LocString TITLE = "Enable Reset Button";
                public static LocString TOOLTIP = "Allows resetting a geyser to it's originally spawned values";
            }
            public class RANDOMIZESLIDERSBUTTON
            {
                public static LocString TITLE = "Enable Randomize Sliders Button";
                public static LocString TOOLTIP = "Sets random values for the 5 primary sliders";
            }
            public class DORMANCYBUTTON
            {
                public static LocString TITLE = "Enable Dormancy Button";
                public static LocString TOOLTIP = "Enters/Exits Dormancy";
            }
            public class ERUPTIONBUTTON
            {
                public static LocString TITLE = "Enable Eruption Button";
                public static LocString TOOLTIP = "Triggers the next scheduled Eruption";
            }
            /*public class MAXPRESSURECONTROLS
            {
                public static LocString TITLE = "Enable Max Pressure Slider";
                public static LocString TOOLTIP = "Allows increasing overpressure limit";
            }*/
            public class UNCAPPRESSURECHECKBOX
            {
                public static LocString TITLE = "Enable Uncap Pressure Checkbox";
                public static LocString TOOLTIP = "Allows 'unlimited' pressure";
            }
            public class ALLOWINSTANTANALYSIS
            {
                public static LocString TITLE = "Enable Instant Analysis";
                public static LocString TOOLTIP = "Analyzing a geyser will complete instantly with no need for a scientist.\nDatabanks still drop";
            }
            public class DISABLECOOLDOWNS
            {
                public static LocString TITLE = "Disable Button Cooldowns and Confirmations";
                public static LocString TOOLTIP = "";
            }
        }
    }
}
