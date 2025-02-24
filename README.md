# GEOBOX - Interlis2 Konverter
Tool für das Konvertieren, Zusammenführen von Interlis2 Dateien.

## Beschrieb
Mit den Datenmodellen DMAV für die Amtliche Vermessung Schweiz wurden einzelne Modelle veröffentlicht, die jeweils ein Thema abdecken. Für einen kompletten Datensatz über sämtliche Themen wurde zusätzlich ein Modell das die weiteren Modelle zusammenfasst veröffentlicht.

Der Prüfservice benötigt nun eine Datendatei (XTF) mit allen Themen. Mit diesem Tool können Themenweise die einzelnen Exportdateien zusammengeführt werden.

> HINWEIS: Das Tool führt keine Interlis-Prüfungen durch. Weder auf korrektes Modell noch auf korrekten Inhalt. Verwende dafür die entsprechenden Tools.

### Konsolen Argumente
Argument | Beschrieb
--- | ---
--type | Art/Typ der Konvertierung die Durchgeführt werden soll. (siehe Funktionen)
--inputDir | Angabe des Verzeichnisses mit den Interlis-Dateien (XTF) zum Konvertieren.
--outputFile | Pfad und Dateiname (inkl. Dateiendung) zur XTF-Dateindatei zum speichern des Resultates. HINWEIS: eine bestehende Datei wird ohne Rückfragen überschrieben.
--logFile | Pfad und Dateiname (inkl. Dateiendung) zur Protokoll-Datei. HINWEIS: eine bestehende Datei wird ohne Rückfragen überschrieben.
--help | Zeigt die Hilfe an.
--version | Zeigt die Versionsinformationen an.

## Vorgehen
### Grundsätzliches Vorgehen
1. Exportiere die Daten aus dem GIS-System als Interlis Datei.
2. Speichere/Kopiere alle Dateien zum zusammenführen oder konvertieren in ein Verzeichnis.
3. Führe die Konvertierung, mit dem entsprechenden Typ, mit diesem Tool durch. In der Regel wird eine Datei erstellt und ein entsprechende Protokoll-Datei (Log-Datei).
4. Kontrolliere die Ausgaben auf der Konsole und in der Protokolldatei.

### Vorgehen für das Datenmodell DMAV
1. Exportiere Themenweise in einzelene Interlis Dateien aus dem GIS-System.
2. Speichere/Kopiere alle Dateien zum zusammenführen in ein Verzeichnis (Achte beim Typ "mergeDMAVfix" auf die korrekten Dateinamen).
3. Führe die Dateien mit diesem Tool zusammen - es wird eine neue Datei erstellt.

## Funktionen
### Typ "mergeDMAVfix"
#### Beispielaufruf
```--type mergeDMAVfix --inputDir "C:\Interlis" --outputFile "C:\Interlis\DMAV_alles.xtf" --logFile "C:\Interlis\DMAV_alles.log"```
### Beschrieb
Es werden die Dateien anhand eines fixen Dateinamen im Verzeichnis gesucht, den Inhalt ausgelesen und zusammengeführt. Die Namespaces werden anhand des Alias gelesen und doppelte Aliase werden entfernt. 

> HINWEIS: die Dateien werden nicht geprüft, ob es das korrekte Modell beinhaltet.

Dateinamen: "DMAV_Bodenbedeckung.xtf", "DMAV_DauerndeBodenverschiebungen.xtf", "DMAV_Dienstbarkeitsgrenzen.xtf", "DMAV_Einzelobjekte.xtf", "KGK_PFDS2.xtf", "DMAV_FixpunkteAVKategorie3.xtf", "FixpunkteLV_LFP.xtf", "FixpunkteLV_HFP.xtf", "DMAV_Gebauudeadressen.xtf", "DMAV_Grundstuecke.xtf", "DMAV_HoheitsgrenzenAV.xtf", "HoheitsgrenzenLV.xtf", "DMAV_Nomenklatur.xtf", "OrtschaftsverzeichnisPLZ.xtf", "DMAV_Rohrleitungen.xtf", "DMAV_Toleranzstufen.xtf", "DMAVSUP_UntereinheitGrundbuch.xtf" 

## Voraussetzungen und Installation
### Voraussetzung
- Microsoft .NET Framework 8

### Installation
- Es benötigt keine Installation.
- Die Applikation kann aus einem beliebigen Verzeichnis aus gestartet werden.
