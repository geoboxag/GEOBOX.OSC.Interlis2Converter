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
--outputFile | Angabe des Datei-Namens (inkl. Dateiendung) für das Resultat, in der Regel eine Interlis-Datei (XTF-Datei). HINWEIS: eine bestehende Datei wird ohne Rückfragen überschrieben.
--outputDir | Angabe des Datei-Pfades (Ordner) für das Speicher des Resultates (Zusammengeführte Interlis (XTF-Datei) oder XTF-Dateien aus dem Download)
--downloadConfig | Angabe des Pfades und Dateiname zur Konfigurationsdatei für den Download der Daten aus den Web-Services.
--logFile | Pfad und Dateiname (inkl. Dateiendung) zur Protokoll-Datei. HINWEIS: eine bestehende Datei wird ohne Rückfragen überschrieben.
--help | Zeigt die Hilfe an.
--version | Zeigt die Versionsinformationen an.

## Vorgehen
### Grundsätzliches Vorgehen
1. Exportiere die Daten aus dem GIS-System als Interlis Datei.
2. Speichere/Kopiere alle Dateien zum zusammenführen oder konvertieren in ein Verzeichnis.
3. [OPTIONAL] Lade die Dateien (XTF-Daten) aus den gewünschten Web-Diensten herunter und speichere diese im selben Verzeichnis.
4. Führe die Konvertierung, mit dem entsprechenden Typ, mit diesem Tool durch. In der Regel wird eine Datei erstellt und eine entsprechende Protokoll-Datei (Log-Datei).
5. Kontrolliere die Ausgaben auf der Konsole und in der Protokolldatei.

### Vorgehen für das Datenmodell DMAV
1. Exportiere Themenweise in einzelene Interlis Dateien aus dem GIS-System.
2. Speichere/Kopiere alle Dateien zum zusammenführen in ein Verzeichnis (Achte beim Typ "mergeDMAVfix" auf die korrekten Dateinamen).
3. Führe die Dateien mit diesem Tool zusammen - es wird eine neue Datei erstellt.

## Funktionen
### Typ "mergeDMAVfix"
#### Beispielaufruf
```--type mergeDMAVfix --inputDir "C:\Interlis" --outputFile "DMAV_alles.xtf" --outputDir "C:\Interlis" --logFile "C:\Interlis\DMAV_alles.log"```
### Beschrieb
Es werden die Dateien anhand eines fixen Dateinamen im Verzeichnis gesucht, den Inhalt ausgelesen und zusammengeführt. Die Namespaces werden anhand des Alias gelesen und doppelte Aliase werden entfernt. 

> HINWEIS: die Dateien werden nicht geprüft, ob es das korrekte Modell beinhaltet.

Dateinamen: "DMAV_Bodenbedeckung.xtf", "DMAV_DauerndeBodenverschiebungen.xtf", "DMAV_Dienstbarkeitsgrenzen.xtf", "DMAV_Einzelobjekte.xtf", "KGK_PFDS2.xtf", "DMAV_FixpunkteAVKategorie3.xtf", "FixpunkteLV_LFP.xtf", "FixpunkteLV_HFP.xtf", "DMAV_Gebauudeadressen.xtf", "DMAV_Grundstuecke.xtf", "DMAV_HoheitsgrenzenAV.xtf", "HoheitsgrenzenLV.xtf", "DMAV_Nomenklatur.xtf", "OrtschaftsverzeichnisPLZ.xtf", "DMAV_Rohrleitungen.xtf", "DMAV_Toleranzstufen.xtf", "DMAVSUP_UntereinheitGrundbuch.xtf" 

### Typ "serviceDownload"
#### Beispielaufruf
```--type serviceDownload --outputDir "C:\Interlis" --downloadConfig "C:\Interlis\OSCConfig\ServiceDownloadConfig.xml" --logFile "C:\Interlis\ServiceDownload.log""```
### Beschrieb
Es werden die Daten von der Angegeben URL heruntergeladen und mit dem gewünschten Name in das Verzeichnis (outputDir) gespeichert.
In der Regel wird eine ZIP-Datei heruntergeladen, sämtliche XTF-Dateien die sich in der ZIP-Datei befinden werden extrahiert. Wenn mehr als eine XTF Dateie gefunden wird, werden die weiteren Dateien mit einem Zeitstempel zu beginn des Dateinamens gekennzeichnet. 

> HINWEIS: die Dateien werden nicht geprüft, ob es das korrekte Modell beinhaltet und werden ohne Rückfragen überschrieben, fals die Datei schon existiert.

### XML Konfiguration
```<!-- Pro Download URL oder XTF-Datei ist ein Eintrag zu erstellen. -->
		<FileDownloadSetting>
			<SourceURL>URL zum Donwload inkl. Datei die Herunterzuladen ist.</SourceURL>
			<FileName>Gewünschter (XTF) Dateiname nach dem Download und aus Extrahieren.</FileName>
		</FileDownloadSetting>```

Beispiel für die Fixpunkte der Kategorie 1 (weitere Beispiele sind in der Sample-Datei aufgeführt):
```<!-- Fixpunkte der Kategorie 1 - geo.admin.ch - Datensatz der swisstopo -->
		<FileDownloadSetting>
			<SourceURL>https://data.geo.admin.ch/ch.swisstopo.fixpunkte-lfp1/fixpunkte-lfp1/fixpunkte-lfp1_2056_5728.xtf.zip</SourceURL>
			<FileName>FixpunkteLV_LFP.xtf</FileName>
		</FileDownloadSetting>```

## Voraussetzungen und Installation
### Voraussetzung
- Microsoft .NET Framework 8

### Installation
- Es benötigt keine Installation.
- Die Applikation kann aus einem beliebigen Verzeichnis aus gestartet werden.
