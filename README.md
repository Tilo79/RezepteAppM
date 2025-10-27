# 🍽️ RezepteApp - Rezeptverwaltungs-App

Eine moderne, plattformübergreifende Rezeptverwaltungs-App entwickelt mit .NET MAUI. Perfekt für Hobby-Köche und Food-Enthusiasten, die ihre Lieblingsrezepte organisieren, Mahlzeiten planen und Einkaufslisten erstellen möchten.

## ✨ Features

### Rezeptverwaltung
- ✅ Rezepte erstellen, bearbeiten und löschen
- ✅ Detaillierte Rezeptinformationen (Zutaten, Anleitung, Zeiten, Portionen)
- ✅ Kategorisierung (Frühstück, Mittagessen, Abendessen, Dessert, Snacks, Getränke)
- ✅ Schwierigkeitsgrade (Einfach, Mittel, Schwer)
- ✅ Nährwertinformationen pro Portion
- ✅ Bewertungssystem (1-5 Sterne)

### Favoritensystem
- ❤️ Rezepte als Favoriten markieren
- 📱 Schneller Zugriff auf Lieblingsrezepte
- 🔄 Einfaches Hinzufügen/Entfernen

### Suche & Filter
- 🔍 Freitext-Suche nach Name und Beschreibung
- 🏷️ Filter nach Kategorie
- ⚡ Filter nach Schwierigkeit
- 🎯 Kombination mehrerer Filter möglich

### Einkaufsliste
- 🛒 Automatische Einkaufsliste aus Rezepten generieren
- ✓ Artikel abhaken beim Einkaufen
- ➕ Manuelle Artikel hinzufügen
- 🗑️ Erledigte Artikel mit einem Klick löschen

### Wochenplanung
- 📅 Mahlzeiten für die Woche planen
- 🍳 Frühstück, Mittagessen, Abendessen organisieren
- 📆 Wochenweise Navigation
- ⏱️ Zeitersparnis durch strukturierte Planung

### Social Features
- 📤 Rezepte teilen
- ⭐ Rezepte bewerten

## 🏗️ Architektur

### Technologie-Stack
- **Framework:** .NET 9 MAUI
- **UI-Pattern:** MVVM (Model-View-ViewModel)
- **Datenbank:** SQLite mit sqlite-net-pcl
- **MVVM Toolkit:** CommunityToolkit.Mvvm
- **UI Toolkit:** CommunityToolkit.Maui

### Projektstruktur
```
RezepteApp/
├── Models/              # Datenmodelle (Recipe, ShoppingListItem, MealPlan)
├── ViewModels/          # MVVM ViewModels mit Command-Implementierungen
├── Views/               # XAML-Seiten für die Benutzeroberfläche
├── Services/            # Business Logic (RecipeService, ShoppingListService, etc.)
├── Data/                # Datenbankzugriff (RecipeDatabase)
├── Converters/          # XAML Value Converters
└── Resources/           # Bilder, Schriften, Styles

```

### Dependency Injection
Alle Services, ViewModels und Views sind über Dependency Injection registriert und werden automatisch aufgelöst.

## 🚀 Installation & Setup

### Voraussetzungen
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (17.8 oder höher) oder [Visual Studio Code](https://code.visualstudio.com/)
- MAUI Workload installiert:
  ```bash
  dotnet workload install maui
  ```

### Projekt klonen und ausführen

1. **Repository klonen**
   ```bash
   git clone <repository-url>
   cd RezepteAppM
   ```

2. **Abhängigkeiten wiederherstellen**
   ```bash
   cd RezepteApp
   dotnet restore
   ```

3. **Projekt bauen**
   ```bash
   dotnet build
   ```

4. **App ausführen**
   
   **Für Windows:**
   ```bash
   dotnet build -t:Run -f net9.0-windows10.0.19041.0
   ```
   
   **Für macOS:**
   ```bash
   dotnet build -t:Run -f net9.0-maccatalyst
   ```
   
   **Für iOS Simulator:**
   ```bash
   dotnet build -t:Run -f net9.0-ios
   ```
   
   **Für Android:**
   ```bash
   dotnet build -t:Run -f net9.0-android
   ```

### Visual Studio
1. Öffnen Sie `RezepteAppM.sln`
2. Wählen Sie die gewünschte Plattform (Windows Machine, Android Emulator, iOS Simulator)
3. Drücken Sie F5 zum Debuggen oder Ctrl+F5 zum Starten ohne Debugger

## 📱 Verwendung

### Erste Schritte
Die App wird mit Beispieldaten initialisiert:
- 5 vorkonfigurierte Rezepte
- Verschiedene Kategorien und Schwierigkeitsgrade
- Bewertungen und Favoriten

### Rezept hinzufügen
1. Navigieren Sie zur "Rezepte"-Seite
2. Tippen Sie auf den blauen ➕-Button
3. Füllen Sie alle Felder aus:
   - Name (Pflichtfeld)
   - Beschreibung
   - Kategorie & Schwierigkeit
   - Zeiten & Portionen
   - Zutaten (mindestens eine)
   - Zubereitungsanleitung
   - Optionale Nährwerte
4. Tippen Sie auf "Speichern"

### Einkaufsliste generieren
1. Öffnen Sie ein Rezept
2. Tippen Sie auf "🛒 Einkauf"
3. Alle Zutaten werden automatisch zur Einkaufsliste hinzugefügt

### Wochenplan erstellen
1. Wechseln Sie zur "Wochenplan"-Seite
2. Tippen Sie auf "+ Mahlzeit hinzufügen"
3. Wählen Sie ein Rezept
4. Wählen Sie die Mahlzeit (Frühstück/Mittagessen/Abendessen)

## 🗄️ Datenbank

Die App verwendet SQLite für lokale Datenspeicherung:
- **Speicherort:** `FileSystem.AppDataDirectory/recipes.db3`
- **Tabellen:** recipes, shopping_list, meal_plans
- **Async Operations:** Alle Datenbankoperationen sind asynchron

### Datenmodelle

**Recipe**
- Basisdaten (Name, Beschreibung, Kategorie, Schwierigkeit)
- Zeitangaben (Vorbereitung, Kochen)
- Zutaten (als JSON-serialisierte Liste)
- Nährwerte (Kalorien, Protein, Kohlenhydrate, Fett)
- Bewertungen und Favoriten-Status

**ShoppingListItem**
- Name, Menge, Einheit
- Checked-Status
- Optionale Rezept-Verknüpfung

**MealPlan**
- Rezept-Verknüpfung
- Datum und Mahlzeittyp
- Portionen

## 🎨 UI/UX Features

- **Tab-Navigation:** Schneller Wechsel zwischen Hauptseiten
- **Pull-to-Refresh:** Aktualisierung der Listen
- **Swipe-Aktionen:** Löschen von Rezepten und Einträgen
- **Responsive Design:** Anpassung an verschiedene Bildschirmgrößen
- **Material Design:** Moderne, benutzerfreundliche Oberfläche

## 🔧 Konfiguration

### Seed Data anpassen
Die Beispieldaten können in `Services/RecipeService.cs` in der Methode `SeedDataAsync()` angepasst werden.

### Kategorien erweitern
Kategorien können in `Models/RecipeCategory.cs` hinzugefügt werden.

### Schwierigkeitsgrade anpassen
Schwierigkeitsgrade können in `Models/RecipeCategory.cs` erweitert werden.

## 🐛 Bekannte Probleme

- **Android SDK:** Benötigt installiertes Android SDK für Android-Build
- **XAML Warnings:** Einige XamlC-Warnungen bezüglich DataBinding (funktional nicht beeinträchtigt)
- **Bilder:** Aktuell werden Platzhalter-Emojis verwendet; Bildupload noch nicht implementiert

## 🚧 Geplante Features

- [ ] Bildupload für Rezepte
- [ ] Rezept-Import/Export (JSON, PDF)
- [ ] Cloud-Synchronisation
- [ ] Offline-Modus
- [ ] Timer für Kochzeiten
- [ ] Schritt-für-Schritt Koch-Modus
- [ ] Kommentare zu Rezepten
- [ ] Rezept-Tags
- [ ] Erweiterte Suchfilter
- [ ] Einkaufsliste nach Kategorien gruppieren
- [ ] Druckfunktion für Rezepte

## 📄 Lizenz

Dieses Projekt ist ein Beispielprojekt und steht unter MIT-Lizenz.

## 👨‍💻 Autor

Entwickelt als vollständige .NET MAUI Beispiel-Applikation.

## 🤝 Beitragen

Contributions, Issues und Feature Requests sind willkommen!

## 📞 Support

Bei Fragen oder Problemen erstellen Sie bitte ein Issue im Repository.

---

**Happy Coding & Guten Appetit! 🍳👨‍🍳**
