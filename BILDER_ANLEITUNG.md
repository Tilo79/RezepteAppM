# 📸 Bilder in RezepteApp einfügen

## 🎯 Option 1: Bilder aus dem Internet (Am einfachsten!)

Du kannst direkt URLs von Bildern aus dem Internet verwenden:

```csharp
new Recipe
{
    Name = "Spaghetti Carbonara",
    ImagePath = "https://images.unsplash.com/photo-1612874742237-6526221588e3",
    // ... weitere Eigenschaften
}
```

### Gute Quellen für kostenlose Bilder:
- **Unsplash**: https://unsplash.com/s/photos/food (komplett kostenlos)
- **Pexels**: https://www.pexels.com/search/food/ (komplett kostenlos)
- **Pixabay**: https://pixabay.com/images/search/food/ (komplett kostenlos)

**Wichtig**: Rechtsklick auf Bild → "Bildadresse kopieren" (nicht die Webseiten-URL!)

---

## 🗂️ Option 2: Lokale Bilder im Projekt speichern

### Schritt 1: Bilder ins Projekt kopieren
1. Erstelle einen Ordner: `RezepteApp/Resources/Images/Recipes/`
2. Kopiere deine Bilder dort hinein (z.B. `carbonara.jpg`, `salat.jpg`)

### Schritt 2: Bilder im CSPROJ registrieren
Öffne `RezepteApp.csproj` und füge hinzu:

```xml
<ItemGroup>
    <MauiImage Include="Resources\Images\Recipes\*.jpg" />
    <MauiImage Include="Resources\Images\Recipes\*.png" />
</ItemGroup>
```

### Schritt 3: Bildpfad im Code verwenden
```csharp
new Recipe
{
    Name = "Spaghetti Carbonara",
    ImagePath = "carbonara.jpg",  // Nur der Dateiname!
    // ... weitere Eigenschaften
}
```

---

## 📱 Option 3: Bilder aus AppData (Benutzer-Upload)

Für Bilder, die Benutzer selbst hochladen:

```csharp
// Bild aus Galerie auswählen
var photo = await MediaPicker.PickPhotoAsync();
if (photo != null)
{
    // Bild in App-Ordner speichern
    var localFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
    
    using var stream = await photo.OpenReadAsync();
    using var newStream = File.OpenWrite(localFilePath);
    await stream.CopyToAsync(newStream);
    
    // Pfad im Rezept speichern
    recipe.ImagePath = localFilePath;
}
```

---

## 🔧 Aktuelle Implementierung

Die App zeigt derzeit **Emojis** (🍝, 🥗, 🥞, 🍛, 🍫) als Platzhalter.

### So änderst du ein Rezept zu einem echten Bild:

1. Öffne: `RezepteApp/Services/RecipeService.cs`
2. Finde das Rezept (z.B. "Spaghetti Carbonara")
3. Ändere die Zeile:

**Vorher:**
```csharp
ImagePath = "🍝",
```

**Nachher (Internet-Bild):**
```csharp
ImagePath = "https://images.unsplash.com/photo-1612874742237-6526221588e3",
```

**Oder (Lokales Bild):**
```csharp
ImagePath = "carbonara.jpg",
```

4. Lösche die Datenbank, damit die Änderungen geladen werden:
   - Windows: `%LOCALAPPDATA%\RezepteApp\recipes.db3`
   - Oder deinstalliere und installiere die App neu

5. Starte die App neu mit Clean-Build:
```powershell
dotnet build -t:Clean,Restore,Build,Run -f net9.0-windows10.0.19041.0
```

---

## 🎨 Beispiel-URLs von Unsplash (kostenlos!)

Hier sind fertige URLs, die du direkt verwenden kannst:

```csharp
// Pasta/Spaghetti
ImagePath = "https://images.unsplash.com/photo-1621996346565-e3dbc646d9a9?w=800"

// Salat
ImagePath = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=800"

// Pancakes
ImagePath = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=800"

// Curry
ImagePath = "https://images.unsplash.com/photo-1588166524941-3bf61a9c41db?w=800"

// Brownies
ImagePath = "https://images.unsplash.com/photo-1607920591413-4ec007e70023?w=800"
```

---

## ✅ Nach dem Ändern

1. **Datenbank löschen**: Die App muss die Seed-Daten neu laden
   - Schnellster Weg: App deinstallieren und neu installieren
   - Oder finde die DB-Datei und lösche sie manuell

2. **Clean Build ausführen**:
```powershell
cd C:\Users\TiloS\RezepteAppM\RezepteApp
dotnet build -t:Clean,Restore,Build,Run -f net9.0-windows10.0.19041.0
```

3. App startet mit echten Bildern! 🎉

---

## 💡 Tipps

- **Bildgröße**: Verwende Bilder mit max. 1920x1080 px (sonst wird die App langsam)
- **Format**: JPG für Fotos, PNG für Grafiken mit Transparenz
- **Internet-Bilder**: Werden beim ersten Laden gecacht, danach schnell
- **Lokale Bilder**: Schneller, aber vergrößern die App-Größe
- **Mix**: Du kannst beides kombinieren!

---

## 🚨 Troubleshooting

### Bild wird nicht angezeigt?
1. Prüfe, ob die URL mit `http://` oder `https://` beginnt
2. Öffne die URL im Browser - wird das Bild angezeigt?
3. Bei lokalen Bildern: Ist der Dateiname richtig (mit Endung .jpg/.png)?

### Alte Emojis werden noch angezeigt?
Die Datenbank hat alte Daten gespeichert. Lösche sie:
- Deinstalliere die App und installiere neu
- Oder lösche: `%LOCALAPPDATA%\RezepteApp\recipes.db3`
