# ♟️ Chess — Desktop Chess Application in C# / WPF

A fully functional two-player chess desktop application built with **C#** and **WPF (.NET 8)**, implementing the complete official FIDE ruleset. The project is structured into two clean layers: a standalone chess engine (`Chess.Core`) and a WPF graphical front-end (`Chess.UI`).

---

## 📸 Screenshots

<!-- PLACEHOLDER: Add a screenshot of the main window here -->
> ![Main Window](screenshots/main_window.png)
> *Main application window — chessboard with side panel*

<!-- PLACEHOLDER: Add a screenshot of a check/endgame state -->
> ![Check Detection](screenshots/check_detection.png)
> *Check detection and move highlighting*

<!-- PLACEHOLDER: Add a screenshot of the pawn promotion dialog -->
> ![Pawn Promotion](screenshots/promotion.png)
> *Pawn promotion dialog*

<!-- PLACEHOLDER: Add a screenshot of the game history panel -->
> ![History Panel](screenshots/replay.png)
> *History and replay mode support*

---

## ✨ Features

### Core Game Logic
- ✅ Full legal move validation for all 6 piece types (Pawn, Rook, Knight, Bishop, Queen, King)
- ✅ Check and checkmate detection via **move simulation**
- ✅ Stalemate detection
- ✅ **Castling** — both kingside and queenside
- ✅ **En Passant** capture
- ✅ **Pawn Promotion** — with a UI dialog for piece selection

### Draw Conditions
- ✅ Stalemate

### UI & UX
- ✅ Interactive WPF board — click to select and move pieces
- ✅ Board flip (swap White/Black perspective)
- ✅ Pawn promotion dialog
- ✅ Game-over screen with result display
- ✅ Side panel with match history

### Data Persistence
- ✅ Game history saved to a local **JSON file** (`game_history.json`) using `Newtonsoft.Json`
- ✅ Move replay — review any past game move by move
- ✅ Graceful I/O error handling (corrupted file recovery, auto-creation on first run)

---

## 🏗️ Architecture

The solution is split into two projects, enforcing separation of concerns:

```
Chess/
├── Chess.Core/         # Chess engine — pure game logic, no UI dependencies
│   ├── Game.cs         # Main game controller (turn management, end-condition checks)
│   ├── Board.cs        # 8×8 board representation, piece placement, IsInCheck
│   ├── Piece.cs        # Abstract base class with GetValidMoves, MoveInDirections, MoveToPositions
│   ├── Move.cs         # Base move class with Execute() and Undo()
│   ├── Position.cs     # Board coordinate struct with equality overrides
│   ├── CastlingMove.cs # Moves king + rook simultaneously; full Undo support
│   ├── EnPassantMove.cs# Removes captured pawn from the skipped square
│   ├── PromotionMove.cs# Replaces pawn with chosen piece on Execute
│   ├── HistoryManager.cs  # Static class: JSON serialization / deserialization
│   ├── GameRecord.cs   # DTO: date, winner, end reason, move list
│   ├── MoveRecord.cs   # DTO: From / To coordinates for history storage
│   └── Enums/          # Player, PieceType, EndReason
│
└── Chess.UI/           # WPF front-end
    ├── MainWindow.xaml / .cs  # Board rendering, mouse events, side panel, dialogs
    └── Images.cs       # Static class: loads piece images into WPF ImageSource objects
```

### Key Design Decisions

**Move simulation for check detection** — instead of maintaining a separate attack map, every candidate move is executed on the real board, `IsInCheck` is queried, and then `Undo()` restores the original state. This keeps the logic simple and provably correct.

**C# generators for move generation** — `GetValidMoves` uses `yield return` (lazy `IEnumerable<Move>`), so the engine only computes moves as they're needed. This reduces memory usage and allows early-exit when, for example, searching for any move that escapes check.

**Strict separation of Core and UI** — `Chess.Core` has zero WPF dependencies and can be tested or reused independently (e.g. for a future AI or web front-end).

---

## 🚀 Getting Started

### Prerequisites
- **Windows** 7 SP1 / 8.1 / 10 / 11
- [**.NET 8.0 Runtime**](https://dotnet.microsoft.com/download/dotnet/8.0) (Desktop)
- [**Visual Studio 2022**](https://visualstudio.microsoft.com/) with the *".NET Desktop Development"* workload

### Run from Source

```bash
git clone https://github.com/Feitasss/Chess.git
cd Chess
```

Open `Chess.sln` in Visual Studio, set `Chess.UI` as the startup project, and press **F5**.

### System Requirements

| Component       | Minimum                              |
|-----------------|--------------------------------------|
| OS              | Windows 7 SP1 or newer               |
| CPU             | 1 GHz                                |
| RAM             | 512 MB                               |
| Disk            | ~50 MB                               |
| Display         | 800 × 600 px                         |

---

## 🛠️ Tech Stack

| Layer       | Technology                         |
|-------------|-------------------------------------|
| Language    | C# (.NET 8)                        |
| UI          | Windows Presentation Foundation (WPF) + XAML |
| Persistence | Newtonsoft.Json (JSON file)        |
| Build       | Visual Studio 2022                 |
| VCS         | Git / GitHub                       |

---

## 🗺️ Roadmap

- [ ] More Draw conditions
- [ ] AI opponent (minimax / alpha-beta pruning)
- [ ] Game clock / time controls
- [ ] PGN import / export
- [ ] Online multiplayer
- [ ] Other gamemodes (Chess 960 etc)

---

## 👤 Author

**[Feitasss](https://github.com/Feitasss)**  
Computer Science student — UITM in Rzeszów  
Skills: C#, Python, WPF, .NET, SQL, Git

---

## 📄 License

This project is open source. Feel free to fork and build on it.
