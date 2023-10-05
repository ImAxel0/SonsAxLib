using UnityEngine;
using SUI;
using TMPro;

using static SUI.SUI;
using AnchorType = SUI.AnchorType;
using Color = UnityEngine.Color;
using SonsSdk;
using RedLoader;
using System;
using System.Drawing;

namespace SonsAxLib;

/// <summary>
/// Personal usage preference of some SUI
/// </summary>
public class AXSUI
{
    private static Vector2 AutoPos(Vector2 size, AnchorType anchorType)
    {
        return anchorType switch
        {
            AnchorType.TopRight => new Vector2(-size.x, -size.y),
            AnchorType.MiddleRight => new Vector2(-size.x, -size.y / 2),
            AnchorType.BottomRight => new Vector2(-size.x, 0),
            AnchorType.BottomCenter => new Vector2(-size.x / 2, 0),
            AnchorType.BottomLeft => new Vector2(0, 0),
            AnchorType.MiddleLeft => new Vector2(0, -size.y / 2),
            AnchorType.TopLeft => new Vector2(0, -size.y),
            AnchorType.TopCenter => new Vector2(-size.x / 2, -size.y),
            AnchorType.MiddleCenter => new Vector2(-size.x / 2, -size.y / 2),
            _ => Vector2.zero,
        };
    }

    /// <summary>
    /// Creates a new panel with auto position adjustment when the anchor is set on screen borders
    /// </summary>
    /// <param name="id"></param>
    /// <param name="size"></param>
    /// <param name="anchorType"></param>
    /// <param name="color"></param>
    /// <param name="style"></param>
    /// <param name="enableInput"></param>
    /// <returns></returns>
    public static SPanelOptions AxCreatePanel(string id, Vector2 size, AnchorType anchorType, Color? color = null, EBackground style = EBackground.None, bool enableInput = false)
    {
        color ??= Color.black;

        Vector2 pos = AutoPos(size, anchorType);

        if (pos == Vector2.zero)
        {
            return (SPanelOptions)RegisterNewPanel(id, enableInput)
                .Pivot(0, 0)
                .Background((Color)color, style)
                .Anchor(anchorType)
                .Size(size.x, size.y);
        }

        return (SPanelOptions)RegisterNewPanel(id, enableInput)
            .Pivot(0, 0)
            .Background((Color)color, style)
            .Anchor(anchorType)
            .Size(size.x, size.y)
            .Position(pos.x, pos.y);
    }

    /// <summary>
    /// Creates a new panel which fills the entire screen
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enableInput"></param>
    /// <returns></returns>
    public static SPanelOptions AxCreateFillPanel(string id, Color? color = null, bool enableInput = false)
    {
        color ??= Color.black;

        return (SPanelOptions)RegisterNewPanel(id, enableInput)
            .Background((Color)color, EBackground.None)
            .Dock(EDockType.Fill);
    }

    /// <summary>
    /// Creates a text in a panel or container
    /// </summary>
    /// <param name="text"></param>
    /// <param name="fontSize"></param>
    /// <param name="alignment"></param>
    /// <returns></returns>
    public static SLabelOptions AxText(string text, int fontSize = 18, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        return SLabel
            .Text(text)
            .FontSize(fontSize).Dock(EDockType.Fill).Alignment(alignment);
    }

    /// <summary>
    /// Creates a text with autosize to fit the panel, container
    /// </summary>
    /// <param name="text"></param>
    /// <param name="alignment"></param>
    /// <returns></returns>
    public static SLabelOptions AxTextAutoSize(string text, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        return SLabel
            .Text(text)
            .FontAutoSize(true).Dock(EDockType.Fill).Alignment(alignment);
    }

    /// <summary>
    /// Creates a text which can be changed dynamically during runtime
    /// </summary>
    /// <param name="text"></param>
    /// <param name="fontSize"></param>
    /// <param name="alignment"></param>
    /// <returns></returns>
    public static SLabelOptions AxTextDynamic(Observable<string> text, int fontSize = 18, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        return SLabel
            .Bind(text)
            .FontSize(fontSize).Dock(EDockType.Fill).Alignment(alignment);
    }

    /// <summary>
    /// Creates a text which can be changed dynamically during runtime with autosize to fit the panel, container
    /// </summary>
    /// <param name="text"></param>
    /// <param name="alignment"></param>
    /// <returns></returns>
    public static SLabelOptions AxTextDynamicAutoSize(Observable<string> text, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        return SLabel
            .Bind(text)
            .FontAutoSize(true).Dock(EDockType.Fill).Alignment(alignment);
    }

    /// <summary>
    /// Creates an interactable button in a panel or container with specified size
    /// </summary>
    /// <param name="label"></param>
    /// <param name="onClick"></param>
    /// <param name="size"></param>
    /// <param name="anchorType"></param>
    /// <returns></returns>
    public static SBgButtonOptions AxButton(string label, Action onClick, Vector2 size, AnchorType anchorType = AnchorType.MiddleCenter)
    {
        Vector2 pos = AutoPos(size, anchorType);

        if (pos == Vector2.zero)
        {
            return SBgButton
            .Pivot(0, 0)
            .Text(label)
            .Anchor(anchorType)
            .Size(size)
            .Notify(onClick);
        }

        return SBgButton
        .Pivot(0, 0)
        .Text(label)
        .Anchor(anchorType)      
        .Size(size)
        .Position(pos.x, pos.y)
        .Notify(onClick);
    }

    public static SBgButtonOptions AxButton(string label, Action<SBgButtonOptions> onClick, Vector2 size, AnchorType anchorType = AnchorType.MiddleCenter)
    {
        Vector2 pos = AutoPos(size, anchorType);

        if (pos == Vector2.zero)
        {
            return SBgButton
            .Pivot(0, 0)
            .Text(label)
            .Anchor(anchorType)
            .Size(size)
            .Notify(onClick);
        }

        return SBgButton
        .Pivot(0, 0)
        .Text(label)
        .Anchor(anchorType)
        .Size(size)
        .Position(pos.x, pos.y)
        .Notify(onClick);
    }

    /// <summary>
    /// Creates an interactable button with specified dock type
    /// </summary>
    /// <param name="label"></param>
    /// <param name="onClick"></param>
    /// <param name="eDockType"></param>
    /// <returns></returns>
    public static SBgButtonOptions AxButton(string label, Action onClick, EDockType eDockType = EDockType.Fill)
    {
        return SBgButton
        .Pivot(0, 0)
        .Text(label)
        .Dock(eDockType)
        .Notify(onClick);
    }

    /// <summary>
    /// Creates a text button like the ones in the main menu
    /// </summary>
    /// <param name="label"></param>
    /// <param name="onClick"></param>
    /// <param name="fontSize"></param>
    /// <returns></returns>
    public static SButtonOptions AxButtonText(string label, Action onClick, int fontSize = 18)
    {
        return SButton
            .Text(label)
            .Dock(EDockType.Fill)
            .FontSize(fontSize)
            .Notify(onClick);
    }

    /// <summary>
    /// Creates a slider of type int in a panel or container
    /// </summary>
    /// <param name="label"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="defaultValue"></param>
    /// <param name="onValueChange"></param>
    /// <returns></returns>
    public static SSliderOptions AxSliderInt(string label, int min, int max, int defaultValue, Action<float> onValueChange = null)
    {
        return SSlider
            .Text(label)
            .Dock(EDockType.Fill)
            .Range(min, max)
            .IntStep()
            .Value(defaultValue)
            .Notify(onValueChange);
    }

    /// <summary>
    /// Creates a slider of type float in a panel or container
    /// </summary>
    /// <param name="label"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="defaultValue"></param>
    /// <param name="onValueChange"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static SSliderOptions AxSliderFloat(string label, float min, float max, float defaultValue, float step = 0.1f, Action<float> onValueChange = null)
    {
        return SSlider
            .Text(label)
            .Dock(EDockType.Fill)
            .Range(min, max)
            .Step(step)
            .Format("0.0")
            .Value(defaultValue)
            .Notify(onValueChange);
    }

    /// <summary>
    /// Creates a input textbox inside a panel or container with a bigger max character limit
    /// </summary>
    /// <param name="label"></param>
    /// <param name="placeholder"></param>
    /// <param name="input"></param>
    /// <param name="onValueChange"></param>
    /// <returns></returns>
    public static STextboxOptions AxInputText(string label, string placeholder, Observable<string> input, Action<string> onValueChange = null)
    {
        var textInput = STextbox
                        .Text(label)
                        .Dock(EDockType.Fill)
                        .Placeholder(placeholder)
                        .Bind(input)
                        .Notify(onValueChange);

        textInput.InputFieldObject.characterLimit = 100000;
        return textInput;
    }

    /// <summary>
    /// Creates an horizontal divider like the ones in game settings
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public static SLabelDividerOptions AxDivider(string label = null)
    {
        return SLabelDivider
            .Text(label)
            .Dock(EDockType.Fill);
    }

    /// <summary>
    /// Used to create a container inside a panel
    /// </summary>
    /// <param name="anchorType"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    public static SContainerOptions AxContainer(AnchorType anchorType, Vector2 size, Color? color = null, EBackground style = EBackground.None)
    {
        color ??= Color.black;
        Vector2 pos = AutoPos(size, anchorType);

        return SContainer
            .Pivot(0, 0)
            .Anchor(anchorType)
            .Size(size)
            .Position(pos.x, pos.y)
            .Background((Color)color, style);
    }

    /// <summary>
    /// Used to access a container of a created NxN panel given the panel id (e.g <see langword="AxCreate2x2Panel"/>)
    /// </summary>
    /// <param name="panelId"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static SContainerOptions AxGetContainerAt(string panelId, int index)
    {
        return (SContainerOptions)GetPanel(panelId)[$"{index}"];
    }

    /// <summary>
    /// Used to access a container of a created NxN panel given the panel itself (e.g <see langword="AxCreate2x2Panel"/>)
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static SContainerOptions AxGetContainerAt(SPanelOptions panel, int index)
    {
        return (SContainerOptions)GetPanel(panel.Id)[$"{index}"];
    }

    internal static SContainerOptions AxGetContainerAt(string panelId, string id)
    {
        return (SContainerOptions)GetPanel(panelId)[id];
    }

    /// <summary>
    /// Creates a 2x2 panel with 4 containers, you can access each container with <see langword="AxGetContainerAt"/>(this panel id, containerIndex) with upper left being 0 then moving horizontally adding 1
    /// </summary>
    /// <param name="id"></param>
    /// <param name="size"></param>
    /// <param name="anchorType"></param>
    /// <param name="color"></param>
    /// <param name="style"></param>
    /// <param name="enableInput"></param>
    /// <returns></returns>
    public static SPanelOptions AxCreate2x2Panel(string id, Vector2 size, AnchorType anchorType, Color? color = null, EBackground style = EBackground.None, bool enableInput = false)
    {
        color ??= Color.black;
        Vector2 pos = AutoPos(size, anchorType);

        var panel = RegisterNewPanel(id, enableInput)
        .Pivot(0, 0)
        .Background((Color)color, style)
        .Anchor(anchorType)
        .Size(size.x, size.y)
        .Position(pos.x, pos.y)
        .LayoutMode("XX");

        panel.Add(AxContainer(AnchorType.TopLeft, new Vector2(size.x / 2, size.y / 2), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), EBackground.RoundedStandard).Id("0"));
        panel.Add(AxContainer(AnchorType.TopRight, new Vector2(size.x / 2, size.y / 2), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), EBackground.RoundedStandard).Id("1"));
        panel.Add(AxContainer(AnchorType.BottomLeft, new Vector2(size.x / 2, size.y / 2), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), EBackground.RoundedStandard).Id("2"));
        panel.Add(AxContainer(AnchorType.BottomRight, new Vector2(size.x / 2, size.y / 2), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), EBackground.RoundedStandard).Id("3"));

        return (SPanelOptions)panel;
    }

    /// <summary>
    /// Creates a 3x3 panel with 9 containers, you can access each container with <see langword="AxGetContainerAt"/>(this panel id, containerIndex) with upper left being 0 then moving horizontally adding 1
    /// </summary>
    /// <param name="id"></param>
    /// <param name="size"></param>
    /// <param name="anchorType"></param>
    /// <param name="color"></param>
    /// <param name="style"></param>
    /// <param name="enableInput"></param>
    /// <returns></returns>
    public static SPanelOptions AxCreate3x3Panel(string id, Vector2 size, AnchorType anchorType, Color? color = null, EBackground style = EBackground.None, bool enableInput = false)
    {
        color ??= Color.black;
        Vector2 pos = AutoPos(size, anchorType);

        var panel = RegisterNewPanel(id, enableInput)
        .Pivot(0, 0)
        .Background((Color)color, style)
        .Anchor(anchorType)
        .Size(size.x, size.y)
        .Position(pos.x, pos.y)
        .LayoutMode("XX");

        panel.Add(AxContainer(AnchorType.TopLeft, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("0"));
        panel.Add(AxContainer(AnchorType.TopCenter, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("1"));
        panel.Add(AxContainer(AnchorType.TopRight, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("2"));
        panel.Add(AxContainer(AnchorType.MiddleLeft, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("3"));
        panel.Add(AxContainer(AnchorType.MiddleCenter, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("4"));
        panel.Add(AxContainer(AnchorType.MiddleRight, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("5"));
        panel.Add(AxContainer(AnchorType.BottomLeft, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("6"));
        panel.Add(AxContainer(AnchorType.BottomCenter, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("7"));
        panel.Add(AxContainer(AnchorType.BottomRight, new Vector2(size.x / 3, size.y / 3), color.Value.WithAlpha(Mathf.Clamp(color.Value.a + 0.2f, 0, 1)), style).Id("8"));

        return (SPanelOptions)panel;
    }

    public static SPanelOptions AxCreateNxNPanel(string id, Vector2 size, AnchorType anchorType, int n, Color? color = null, EBackground style = EBackground.None, bool enableInput = false)
    {
        color ??= Color.black;
        Vector2 pos = AutoPos(size, anchorType);

        var panel = RegisterNewPanel(id, enableInput)
        .Pivot(0, 0)
        .Background((Color)color, style)
        .Anchor(anchorType)
        .Size(size.x, size.y)
        .Position(pos.x, pos.y)
        .Vertical(0, "EX");
        
        // creating each horizontal container
        for (int i = 0; i < n; i++)
        {
            panel.Add(SContainer).Background(Color.cyan.WithAlpha(0.4f), EBackground.None).Id($"n{i}");
        }
        
        // each horizontal container
        for (int i = 0; i < n; i++)
        {
            SContainerOptions sContainer = AxGetContainerAt(id, $"n{i}");
            if (sContainer == null) RLog.Warning("was null");
            /*
            // each box container
            for (int j = 0; j < n; j++)
            {
                sContainer.Add(SContainer.Size(size.x / n, size.y / n).Background(Color.cyan));
            }
            */
        }
        
        return (SPanelOptions)panel;
    }

    /// <summary>
    /// <para>Gets the main container of next panels to which you can add other AxMenuComponents (e.g <see langword="AxMenuSlider"/>) or SUI elements</para>
    /// <para>Used for: <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/></para>
    /// </summary>
    /// <param name="panelId"></param>
    /// <returns></returns>
    public static SScrollContainerOptions AxGetMainContainer(string panelId)
    {
        var scrollbar = (SContainerOptions)GetPanel(panelId)["scrollbar"];
        return (SScrollContainerOptions)scrollbar["scrollcontainer"];
    }

    /// <summary>
    /// <para>Gets the main container of next panels to which you can add other AxMenuComponents (e.g <see langword="AxMenuSlider"/>) or SUI elements</para>
    /// <para>Used for: <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/></para>
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    public static SScrollContainerOptions AxGetMainContainer(SPanelOptions panel)
    {
        var scrollbar = (SContainerOptions)GetPanel(panel.Id)["scrollbar"];
        return (SScrollContainerOptions)scrollbar["scrollcontainer"];
    }

    /// <summary>
    /// Gets the title of a <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/>
    /// </summary>
    /// <param name="panelId"></param>
    /// <returns></returns>
    public static SLabelOptions AxGetMenuTitle(string panelId)
    {
        return (SLabelOptions)GetPanel(panelId)["menutitle"];
    }

    /// <summary>
    /// Gets the title of a <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/>
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    public static SLabelOptions AxGetMenuTitle(SPanelOptions panel)
    {
        return (SLabelOptions)GetPanel(panel.Id)["menutitle"];
    }

    /// <summary>
    /// Creates a vertical settings like menu panel with scrollbar to which you can add sliders, input text etc.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    /// <param name="style"></param>
    /// <param name="enableInput"></param>
    /// <returns></returns>
    public static SPanelOptions AxCreateMenuPanel(string id, string title, Vector2 size, Color? color = null, EBackground style = EBackground.RoundedStandard, bool enableInput = true)
    {
        color ??= Color.black.WithAlpha(0.95f);

        var menuPanel = AxCreatePanel(id, size, AnchorType.MiddleCenter, color, style, enableInput).Vertical(2, "EC").Padding(2);

        var titleContainer = SContainer.Background((Color)color, EBackground.None).PaddingVertical(10).PHeight(size.y / 10)
            - AxTextAutoSize(title).Id("menutitle")
            - AxButton("X", CloseMenuPanel, new Vector2(size.x / 10, size.y / 10), AnchorType.MiddleRight).Background(EBackground.RoundedStandard);
        menuPanel.Add(titleContainer);

        var scrollBar = SDiv.FlexHeight(1);
        scrollBar.Id("scrollbar");
        menuPanel.Add(scrollBar);

        var settingsScroll = SScrollContainer
        .Dock(EDockType.Fill)
        .Background(Color.black.WithAlpha(0), EBackground.RoundedStandard)
        .Size(-20, -20)
        .As<SScrollContainerOptions>();
        settingsScroll.ContainerObject.Spacing(10);
        settingsScroll.ContainerObject.PaddingHorizontal(10);
        settingsScroll.Id("scrollcontainer");
        scrollBar.Add(settingsScroll);

        var bottomContainer = SContainer.Background((Color)color, EBackground.None).PaddingVertical(10).PHeight(size.y / 15);
        menuPanel.Add(bottomContainer);

        return (SPanelOptions)menuPanel;
    }

    internal static void CloseMenuPanel()
    {
        TogglePanel("", false);
    }

    /// <summary>
    /// Creates an <see langword="int"/> slider with a background container, mainly used with <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/>
    /// </summary>
    /// <param name="label"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="defaultValue"></param>
    /// <param name="onValueChange"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static SContainerOptions AxMenuSliderInt(string label, int min, int max, int defaultValue, Action<float> onValueChange = null, float height = 50f)
    {
        return SContainer.Background(Color.black.WithAlpha(0.9f), EBackground.None).PHeight(height)
            - AxSliderInt(label, min, max, defaultValue, onValueChange).HOffset(10, -10);
    }

    /// <summary>
    /// Creates a <see langword="float"/> slider with a background container, mainly used with <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/>
    /// </summary>
    /// <param name="label"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="defaultValue"></param>
    /// <param name="step"></param>
    /// <param name="onValueChange"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static SContainerOptions AxMenuSliderFloat(string label, float min, float max, float defaultValue, float step = 0.1f, Action<float> onValueChange = null, float height = 50f)
    {
        return SContainer.Background(Color.black.WithAlpha(0.9f), EBackground.None).PHeight(height)
            - AxSliderFloat(label, min, max, defaultValue, step, onValueChange).HOffset(10, -10);
    }

    /// <summary>
    /// Creates an input text box with a background container, mainly used with <see langword="AxCreateMenuPanel"/>, <see langword="AxCreateScrollBorderPanel"/>
    /// </summary>
    /// <param name="label"></param>
    /// <param name="placeHolder"></param>
    /// <param name="input"></param>
    /// <param name="onValueChange"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static SContainerOptions AxMenuInputText(string label, string placeHolder, Observable<string> input, Action<string> onValueChange = null, float height = 50f)
    {
        return SContainer.Background(Color.black.WithAlpha(0.9f), EBackground.None).PHeight(height)
            - AxInputText(label, placeHolder, input, onValueChange).HOffset(10, -10);
    }

    public enum Side
    {
        Left,
        Right
    }

    /// <summary>
    /// Creates a vertical panel on a side of the screen which fills it vertically which you can add sliders, input text etc.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="side"></param>
    /// <param name="hSize"></param>
    /// <param name="color"></param>
    /// <param name="style"></param>
    /// <param name="enableInput"></param>
    /// <returns></returns>
    public static SPanelOptions AxCreateScrollBorderPanel(string id, string title, Side side, float hSize, Color? color = null, EBackground style = EBackground.None, bool enableInput = false)
    {
        color ??= Color.black.WithAlpha(0.95f);
        AnchorType anchorType = (side == Side.Left) ? AnchorType.TopLeft : AnchorType.TopRight;
        var pos = (anchorType == AnchorType.TopLeft) ? new Vector2(0, -0) : new Vector2(-hSize, -0);

        var sidePanel = RegisterNewPanel(id, enableInput)
            .Pivot(0, 0)
            .Anchor(anchorType)
            .Background((Color)color, style)
            .Size(hSize)
            .Position(pos.x, pos.y)
            .Vertical(5, "EC")
            .VFill();

        var titleContainer = SContainer.Background((Color)color, EBackground.None).PaddingVertical(10).PHeight(50)
            - AxTextAutoSize(title).Id("menutitle")
            - AxButton("X", CloseMenuPanel, new Vector2(50, 50), AnchorType.MiddleRight).Background(EBackground.RoundedStandard);
        sidePanel.Add(titleContainer);

        var scrollBar = SDiv.FlexHeight(1);
        scrollBar.Id("scrollbar");
        sidePanel.Add(scrollBar);

        var settingsScroll = SScrollContainer
        .Dock(EDockType.Fill)
        .Background(Color.black.WithAlpha(0), EBackground.RoundedStandard)
        .Size(-20, -20)
        .As<SScrollContainerOptions>();
        settingsScroll.ContainerObject.Spacing(10);
        settingsScroll.ContainerObject.PaddingHorizontal(10);
        settingsScroll.Id("scrollcontainer");
        scrollBar.Add(settingsScroll);

        return (SPanelOptions)sidePanel;
    }
}
