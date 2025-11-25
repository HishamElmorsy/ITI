# 🍑 Apricot Chat - AI File Analyzer

## What's New

This is the **styled version** of the AI File Analyzer with the beautiful Apricot theme!

### ✨ Features

- **Simple & Working Logic**: Uses the proven semantic chunking + batch embeddings approach
- **Beautiful UI**: Apricot-themed gradient background, styled cards, and modern design (no navbar/footer clutter!)
- **OpenAI Only**: Uses only OpenAI API (gpt-4o-mini for chat, text-embedding-3-large for embeddings)
- **File Support**: TXT, PDF, DOCX with proper text extraction
- **Multi-File Support**: Upload multiple files and switch between them with a dropdown
- **Smart Upload UI**: Upload form hides after success, with "Upload New File" button in success message
- **Similarity Scores**: Shows relevance percentage with color-coded badges
  - Green (≥70%): High relevance
  - Yellow (40-69%): Medium relevance
  - Gray (<40%): Low relevance

### 🚀 How to Run

1. **Open the project** in Visual Studio or VS Code:
   ```
   C:\Users\aisho\Downloads\lab04\AiFileAnalyzerMvc\AiFileAnalyzerMvc.sln
   ```

2. **Your OpenAI API key is already configured** in `appsettings.json`

3. **Run the project** (F5 or `dotnet run`)

4. **Use the app**:
   - Upload a file (TXT, PDF, or DOCX)
   - Ask questions about the file
   - See answers with relevance scores

### 📁 Project Structure

```
Services/
  ├── OpenAIService.cs          # Handles embeddings + chat completions
  └── FileProcessingService.cs  # Extracts text from files

Controllers/
  └── ChatController.cs          # Upload & Ask endpoints

Views/
  └── Chat/Index.cshtml          # Beautiful Apricot-themed UI

Models/
  ├── FileData.cs               # Uploaded file metadata
  ├── OpenAIResponse.cs         # API response model
  └── ChatMessage.cs            # Chat message model
```

### 🎨 Styling

The Apricot theme uses:
- **Primary Color**: #FFB347 (warm apricot orange)
- **Background**: Soft gradient with radial overlays
- **Cards**: Semi-transparent white with shadows
- **Messages**: User (white bg), Assistant (apricot-100 bg)

### 🔧 Configuration

Edit `appsettings.json` to change the OpenAI API key:

```json
{
  "OpenAI": {
    "ApiKey": "your-key-here"
  }
}
```

### ✅ What Works

- ✅ File upload (TXT, PDF, DOCX)
- ✅ Multiple file management with dropdown selector
- ✅ Smart upload form (hides after success)
- ✅ Text extraction (iText7 for PDF, OpenXML for DOCX)
- ✅ Semantic chunking by sentences
- ✅ Batch embeddings (all chunks + question in one API call)
- ✅ Cosine similarity to find best chunk
- ✅ OpenAI chat completion with context
- ✅ Similarity percentage display
- ✅ Beautiful, responsive UI (no navbar/footer)

### 📝 Notes

- Files are stored in `wwwroot/uploads/`
- Each file gets a unique GUID prefix to avoid name conflicts
- The app uses a static variable to store the current file (single-user session)
- For multi-user support, consider using a database or session storage

---

**Enjoy your Apricot Chat! 🍑**
