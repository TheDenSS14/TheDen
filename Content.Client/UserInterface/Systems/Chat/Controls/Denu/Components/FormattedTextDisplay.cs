// SPDX-FileCopyrightText: 2025 Cam
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.ContentPack;
using Robust.Shared.IoC;
using Robust.Shared.Utility;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu.Components;


public sealed class FormattedTextDisplay : PanelContainer
{
    private readonly OutputPanel _outputPanel;
    private IResourceManager? _resourceManager;
    private Func<string, string>? _formatter;
    private string? _rawText;
    private string? _lastFormattedText;
    private string? _filePath;

    public Func<string, string>? Formatter
    {
        get => _formatter;
        set
        {
            _formatter = value;
            ForceReformat();
        }
    }

    public string Text
    {
        get => _rawText ?? string.Empty;
        set
        {
            if (_rawText == value)
                return;

            _rawText = value;
            UpdateFormattedText();
        }
    }

    public string? FilePath
    {
        get => _filePath;
        set
        {
            if (_filePath == value)
                return;

            _filePath = value;
            LoadTextFromFile();
        }
    }

    public FormattedTextDisplay()
    {
        _outputPanel = new OutputPanel { ScrollFollowing = false };
        AddChild(_outputPanel);
    }

    public void ForceReformat()
    {
        _lastFormattedText = null;
        UpdateFormattedText();
    }

    private void LoadTextFromFile()
    {
        if (string.IsNullOrEmpty(_filePath))
        {
            Text = string.Empty;
            return;
        }

        try
        {
            _resourceManager ??= IoCManager.Resolve<IResourceManager>();
            string fileContent = _resourceManager.ContentFileReadAllText(_filePath);
            Text = fileContent;
        }
        catch (Exception)
        {
            Text = $"Failed to load content from: {_filePath}";
        }
    }

    private void UpdateFormattedText()
    {
        if (string.IsNullOrEmpty(_rawText))
        {
            _outputPanel.Clear();
            _lastFormattedText = null;
            return;
        }

        string formattedText = _formatter?.Invoke(_rawText) ?? _rawText;

        if (_lastFormattedText == formattedText)
            return;
        
        _lastFormattedText = formattedText;

        FormattedMessage message = FormattedMessage.FromMarkupOrThrow(formattedText);
        _outputPanel.Clear();
        _outputPanel.AddMessage(message);
    }
}
