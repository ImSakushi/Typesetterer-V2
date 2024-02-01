using FlatUI;
using System.Windows.Forms;

namespace Typesetterer.Data
{
    public class KeyboardShortcuts
    {
        public ShortcutKeys AdvanceStep { get; } = new ShortcutKeys(Keys.E, "Move to next step in phase.");

        public Keys AdvanceStepKeys
        {
            get
            {
                return this.AdvanceStep.Value;
            }
            set
            {
                this.AdvanceStep.Value = value;
            }
        }

        public ShortcutKeys CancelDialog { get; } = new ShortcutKeys(Keys.Escape, "Cancel Dialog.");

        public Keys CancelDialogKeys
        {
            get
            {
                return this.CancelDialog.Value;
            }
            set
            {
                this.CancelDialog.Value = value;
            }
        }

        public KeyboardShortcuts.CreateTextDialogKeys CreateTextDialog
        {
            get;
            set;
        }

        public KeyboardShortcuts.OverlayKeys Overlay
        {
            get;
            set;
        }

        public KeyboardShortcuts.PrototyperKeys Prototyper
        {
            get;
            set;
        }

        public ShortcutKeys RetreatStep { get; } = new ShortcutKeys(Keys.LButton | Keys.MButton | Keys.XButton1 | Keys.A | Keys.D | Keys.E | Keys.Shift, "Move to previous step in phase.");

        public Keys RetreatStepKeys
        {
            get
            {
                return this.RetreatStep.Value;
            }
            set
            {
                this.RetreatStep.Value = value;
            }
        }

        public ShortcutKeys SwitchBetweenPrototyperAndOverlay { get; } = new ShortcutKeys(Keys.Oemtilde, "Transition between overlay and prototyper.");

        public Keys SwitchBetweenPrototyperAndOverlayKeys
        {
            get
            {
                return this.SwitchBetweenPrototyperAndOverlay.Value;
            }
            set
            {
                this.SwitchBetweenPrototyperAndOverlay.Value = value;
            }
        }

        public KeyboardShortcuts()
        {
            this.Prototyper = new KeyboardShortcuts.PrototyperKeys();
            this.Overlay = new KeyboardShortcuts.OverlayKeys();
            this.CreateTextDialog = new KeyboardShortcuts.CreateTextDialogKeys();
        }

        public override string ToString()
        {
            return "Application";
        }

        public class CreateTextDialogKeys
        {
            public ShortcutKeys EditCurrentLine
            {
                get;
            }

            public Keys EditCurrentLineKeys
            {
                get
                {
                    return this.EditCurrentLine.Value;
                }
                set
                {
                    this.EditCurrentLine.Value = value;
                }
            }

            public ShortcutKeys EnterTextSize
            {
                get;
            }

            public Keys EnterTextSizeKeys
            {
                get
                {
                    return this.EnterTextSize.Value;
                }
                set
                {
                    this.EnterTextSize.Value = value;
                }
            }

            public ShortcutKeys MoveTextDownBig
            {
                get;
            }

            public Keys MoveTextDownBigKeys
            {
                get
                {
                    return this.MoveTextDownBig.Value;
                }
                set
                {
                    this.MoveTextDownBig.Value = value;
                }
            }

            public ShortcutKeys MoveTextDownSmall
            {
                get;
            }

            public Keys MoveTextDownSmallKeys
            {
                get
                {
                    return this.MoveTextDownSmall.Value;
                }
                set
                {
                    this.MoveTextDownSmall.Value = value;
                }
            }

            public ShortcutKeys MoveTextLeftBig
            {
                get;
            }

            public Keys MoveTextLeftBigKeys
            {
                get
                {
                    return this.MoveTextLeftBig.Value;
                }
                set
                {
                    this.MoveTextLeftBig.Value = value;
                }
            }

            public ShortcutKeys MoveTextLeftSmall
            {
                get;
            }

            public Keys MoveTextLeftSmallKeys
            {
                get
                {
                    return this.MoveTextLeftSmall.Value;
                }
                set
                {
                    this.MoveTextLeftSmall.Value = value;
                }
            }

            public ShortcutKeys MoveTextRightBig
            {
                get;
            }

            public Keys MoveTextRightBigKeys
            {
                get
                {
                    return this.MoveTextRightBig.Value;
                }
                set
                {
                    this.MoveTextRightBig.Value = value;
                }
            }

            public ShortcutKeys MoveTextRightSmall
            {
                get;
            }

            public Keys MoveTextRightSmallKeys
            {
                get
                {
                    return this.MoveTextRightSmall.Value;
                }
                set
                {
                    this.MoveTextRightSmall.Value = value;
                }
            }

            public ShortcutKeys MoveTextUpBig
            {
                get;
            }

            public Keys MoveTextUpBigKeys
            {
                get
                {
                    return this.MoveTextUpBig.Value;
                }
                set
                {
                    this.MoveTextUpBig.Value = value;
                }
            }

            public ShortcutKeys MoveTextUpSmall
            {
                get;
            }

            public Keys MoveTextUpSmallKeys
            {
                get
                {
                    return this.MoveTextUpSmall.Value;
                }
                set
                {
                    this.MoveTextUpSmall.Value = value;
                }
            }

            public ShortcutKeys MoveToNextGroup
            {
                get;
            }

            public Keys MoveToNextGroupKeys
            {
                get
                {
                    return this.MoveToNextGroup.Value;
                }
                set
                {
                    this.MoveToNextGroup.Value = value;
                }
            }

            public ShortcutKeys MoveToPreviousGroup
            {
                get;
            }

            public Keys MoveToPreviousGroupKeys
            {
                get
                {
                    return this.MoveToPreviousGroup.Value;
                }
                set
                {
                    this.MoveToPreviousGroup.Value = value;
                }
            }

            public ShortcutKeys SelectFifthLayout
            {
                get;
            }

            public Keys SelectFifthLayoutKeys
            {
                get
                {
                    return this.SelectFifthLayout.Value;
                }
                set
                {
                    this.SelectFifthLayout.Value = value;
                }
            }

            public ShortcutKeys SelectFirstLayout
            {
                get;
            }

            public Keys SelectFirstLayoutKeys
            {
                get
                {
                    return this.SelectFirstLayout.Value;
                }
                set
                {
                    this.SelectFirstLayout.Value = value;
                }
            }

            public ShortcutKeys SelectFourthLayout
            {
                get;
            }

            public Keys SelectFourthLayoutKeys
            {
                get
                {
                    return this.SelectFourthLayout.Value;
                }
                set
                {
                    this.SelectFourthLayout.Value = value;
                }
            }

            public ShortcutKeys SelectSecondLayout
            {
                get;
            }

            public Keys SelectSecondLayoutKeys
            {
                get
                {
                    return this.SelectSecondLayout.Value;
                }
                set
                {
                    this.SelectSecondLayout.Value = value;
                }
            }

            public ShortcutKeys SelectThirdLayout
            {
                get;
            }

            public Keys SelectThirdLayoutKeys
            {
                get
                {
                    return this.SelectThirdLayout.Value;
                }
                set
                {
                    this.SelectThirdLayout.Value = value;
                }
            }

            public ShortcutKeys ToggleCalculatedMidpoint
            {
                get;
            }

            public Keys ToggleCalculatedMidpointKeys
            {
                get
                {
                    return this.ToggleCalculatedMidpoint.Value;
                }
                set
                {
                    this.ToggleCalculatedMidpoint.Value = value;
                }
            }

            public ShortcutKeys ToggleShowInfo
            {
                get;
            }

            public Keys ToggleShowInfoKeys
            {
                get
                {
                    return this.ToggleShowInfo.Value;
                }
                set
                {
                    this.ToggleShowInfo.Value = value;
                }
            }

            public CreateTextDialogKeys()
            {
            }

            public override string ToString()
            {
                return "Create Text Dialog";
            }
        }

        public class OverlayKeys
        {
            public ShortcutKeys ApplyCurrentTextStyle
            {
                get;
            }

            public Keys ApplyCurrentTextStyleKeys
            {
                get
                {
                    return this.ApplyCurrentTextStyle.Value;
                }
                set
                {
                    this.ApplyCurrentTextStyle.Value = value;
                }
            }

            public ShortcutKeys ApplyNextTextStyle
            {
                get;
            }

            public Keys ApplyNextTextStyleKeys
            {
                get
                {
                    return this.ApplyNextTextStyle.Value;
                }
                set
                {
                    this.ApplyNextTextStyle.Value = value;
                }
            }

            public ShortcutKeys ApplyPreviousTextStyle
            {
                get;
            }

            public Keys ApplyPreviousTextStyleKeys
            {
                get
                {
                    return this.ApplyPreviousTextStyle.Value;
                }
                set
                {
                    this.ApplyPreviousTextStyle.Value = value;
                }
            }

            public ShortcutKeys AutoCenterText
            {
                get;
            }

            public Keys AutoCenterTextKeys
            {
                get
                {
                    return this.AutoCenterText.Value;
                }
                set
                {
                    this.AutoCenterText.Value = value;
                }
            }

            public ShortcutKeys DecreaseTextSize
            {
                get;
            }

            public Keys DecreaseTextSizeKeys
            {
                get
                {
                    return this.DecreaseTextSize.Value;
                }
                set
                {
                    this.DecreaseTextSize.Value = value;
                }
            }

            public ShortcutKeys FinishEnteringText
            {
                get;
            }

            public Keys FinishEnteringTextKeys
            {
                get
                {
                    return this.FinishEnteringText.Value;
                }
                set
                {
                    this.FinishEnteringText.Value = value;
                }
            }

            public ShortcutKeys IncreaseTextSize
            {
                get;
            }

            public Keys IncreaseTextSizeKeys
            {
                get
                {
                    return this.IncreaseTextSize.Value;
                }
                set
                {
                    this.IncreaseTextSize.Value = value;
                }
            }

            public ShortcutKeys NavigateNextOrMoveDownLine
            {
                get;
            }

            public Keys NavigateNextOrMoveDownLineKeys
            {
                get
                {
                    return this.NavigateNextOrMoveDownLine.Value;
                }
                set
                {
                    this.NavigateNextOrMoveDownLine.Value = value;
                }
            }

            public ShortcutKeys NavigatePreviousOrMoveUpLine
            {
                get;
            }

            public Keys NavigatePreviousOrMoveUpLineKeys
            {
                get
                {
                    return this.NavigatePreviousOrMoveUpLine.Value;
                }
                set
                {
                    this.NavigatePreviousOrMoveUpLine.Value = value;
                }
            }

            public ShortcutKeys ToggleArtLayers
            {
                get;
            }

            public Keys ToggleArtLayersKeys
            {
                get
                {
                    return this.ToggleArtLayers.Value;
                }
                set
                {
                    this.ToggleArtLayers.Value = value;
                }
            }

            public ShortcutKeys TypeCurrentLine
            {
                get;
            }

            public Keys TypeCurrentLineKeys
            {
                get
                {
                    return this.TypeCurrentLine.Value;
                }
                set
                {
                    this.TypeCurrentLine.Value = value;
                }
            }

            public OverlayKeys()
            {
            }

            public override string ToString()
            {
                return "Overlay";
            }
        }

        public class PrototyperKeys
        {
            public ShortcutKeys MoveBackInHistory
            {
                get;
            }

            public Keys MoveBackInHistoryKeys
            {
                get
                {
                    return this.MoveBackInHistory.Value;
                }
                set
                {
                    this.MoveBackInHistory.Value = value;
                }
            }

            public ShortcutKeys MoveNextTextLine
            {
                get;
            }

            public Keys MoveNextTextLineKeys
            {
                get
                {
                    return this.MoveNextTextLine.Value;
                }
                set
                {
                    this.MoveNextTextLine.Value = value;
                }
            }

            public ShortcutKeys MovePreviousTextLine
            {
                get;
            }

            public Keys MovePreviousTextLineKeys
            {
                get
                {
                    return this.MovePreviousTextLine.Value;
                }
                set
                {
                    this.MovePreviousTextLine.Value = value;
                }
            }

            public ShortcutKeys NavigateNext
            {
                get;
            }

            public Keys NavigateNextKeys
            {
                get
                {
                    return this.NavigateNext.Value;
                }
                set
                {
                    this.NavigateNext.Value = value;
                }
            }

            public ShortcutKeys NavigatePrevious
            {
                get;
            }

            public Keys NavigatePreviousKeys
            {
                get
                {
                    return this.NavigatePrevious.Value;
                }
                set
                {
                    this.NavigatePrevious.Value = value;
                }
            }

            public ShortcutKeys SwitchToMagicWand
            {
                get;
            }

            public Keys SwitchToMagicWandKeys
            {
                get
                {
                    return this.SwitchToMagicWand.Value;
                }
                set
                {
                    this.SwitchToMagicWand.Value = value;
                }
            }

            public ShortcutKeys SwitchToMarquee
            {
                get;
            }

            public Keys SwitchToMarqueeKeys
            {
                get
                {
                    return this.SwitchToMarquee.Value;
                }
                set
                {
                    this.SwitchToMarquee.Value = value;
                }
            }

            public ShortcutKeys SwitchToPoint
            {
                get;
            }

            public Keys SwitchToPointKeys
            {
                get
                {
                    return this.SwitchToPoint.Value;
                }
                set
                {
                    this.SwitchToPoint.Value = value;
                }
            }

            public ShortcutKeys ToggleHistoryStep
            {
                get;
            }

            public Keys ToggleHistoryStepKeys
            {
                get
                {
                    return this.ToggleHistoryStep.Value;
                }
                set
                {
                    this.ToggleHistoryStep.Value = value;
                }
            }

            public ShortcutKeys ToggleShowInfo
            {
                get;
            }

            public Keys ToggleShowInfoKeys
            {
                get
                {
                    return this.ToggleShowInfo.Value;
                }
                set
                {
                    this.ToggleShowInfo.Value = value;
                }
            }

            public PrototyperKeys()
            {
            }

            public override string ToString()
            {
                return "Prototyper";
            }
        }
    }
}