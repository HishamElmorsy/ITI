# 🍑 Apricot Chat - Feature Guide

## UI Improvements

### 1. **Clean Layout** ✨
- ❌ Removed navbar
- ❌ Removed footer
- ✅ Full-screen chat experience
- ✅ Centered card with Apricot theme

### 2. **Smart Upload Form** 📤

**Before Upload:**
```
┌─────────────────────────────────────┐
│ 🍑 Apricot Chat - AI File Analyzer │
├─────────────────────────────────────┤
│ [Choose File] [📤 Upload File]     │
│ Supported: TXT, PDF, DOCX           │
│                                     │
│ Upload a file and start asking...  │
└─────────────────────────────────────┘
```

**After Upload:**
```
┌─────────────────────────────────────┐
│ 🍑 Apricot Chat - AI File Analyzer │
├─────────────────────────────────────┤
│ ✅ File 'document.pdf' uploaded!    │
│                [📤 Upload New File] │
│                                     │
│ (Upload form is hidden)             │
│                                     │
│ Chat messages appear here...        │
└─────────────────────────────────────┘
```

### 3. **Multi-File Support** 📁

**When you have multiple files:**
```
┌─────────────────────────────────────┐
│ ✅ File 'report.docx' uploaded!     │
│                [📤 Upload New File] │
├─────────────────────────────────────┤
│ Select a file to chat with:        │
│ [▼ report.docx ▼]                  │
│   • document.pdf                    │
│   • notes.txt                       │
│   • report.docx (selected)          │
└─────────────────────────────────────┘
```

### 4. **Styled Chat Messages** 💬

**User Message:**
```
┌─────────────────────────────────────┐
│ You                                 │
│ ┌─────────────────────────────────┐ │
│ │ What is the main topic?         │ │
│ └─────────────────────────────────┘ │
│     (white background)              │
└─────────────────────────────────────┘
```

**Assistant Response:**
```
┌─────────────────────────────────────┐
│ Assistant [Relevant: 85.3%] 🟢     │
│ ┌─────────────────────────────────┐ │
│ │ The main topic is...            │ │
│ └─────────────────────────────────┘ │
│     (apricot background)            │
└─────────────────────────────────────┘
```

## How It Works

### Upload Flow
1. Click "Choose File" → Select TXT/PDF/DOCX
2. Click "📤 Upload File"
3. ✅ Success message appears
4. Upload form hides automatically
5. File selector appears (if multiple files)

### Chat Flow
1. Type question in input box
2. Click "Send"
3. Your message appears (white background)
4. AI response appears (apricot background)
5. Relevance badge shows confidence:
   - 🟢 Green (≥70%): High confidence
   - 🟡 Yellow (40-69%): Medium confidence
   - ⚪ Gray (<40%): Low confidence

### Switch Files
1. Upload multiple files
2. Use dropdown: "Select a file to chat with"
3. Choose different file
4. Chat continues with new file context

### Upload Another File
1. Click "📤 Upload New File" button in success message
2. Upload form reappears
3. Select and upload new file
4. New file added to dropdown

## Technical Details

- **Session Storage**: Uses static variables (single-user)
- **File Tracking**: List of all uploaded files
- **Current File**: Active file for chat context
- **Auto-Switch**: Selecting from dropdown switches context immediately

## Benefits

✅ **Cleaner UI**: No distracting navbar/footer
✅ **Better UX**: Upload form only shows when needed
✅ **Multi-File**: Easy switching between documents
✅ **Visual Feedback**: Clear success messages and badges
✅ **Simple**: One-page app, no navigation needed

---

**Ready to use! Just run the project and start chatting with your files! 🍑**
