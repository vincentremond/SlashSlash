# SlashSlash

A command-line utility for transforming clipboard content with various useful operations.

## Features

- **Conventional Commit Branch Name Generation**: Automatically converts conventional commit messages into branch names
  - Example: `feat(scope): add new feature` → `feat/add-new-feature--scope`

- **Bookmarklet Decoder**: Decodes JavaScript bookmarklets by removing the wrapper and URL-decoding the content
  - Example: `javascript:(function(){alert('hello');})();` → `alert('hello');`

- **JSON Transformations**: Multiple JSON-related transformations including:
  - Simple JSON serialization
  - JSON with trimmed double quotes
  - Double JSON serialization
  - Custom quote handling variations

- **Regex Operations**:
  - Regex escape
  - Combined regex escape with JSON serialization

## Usage

1. Copy the content you want to transform to your clipboard
2. Run the application
3. Select the desired transformation from the interactive menu
4. The transformed content will be automatically copied to your clipboard

## Dependencies

- Spectre.Console - For interactive console UI
- Newtonsoft.Json - For JSON operations
- TextCopy - For clipboard operations
