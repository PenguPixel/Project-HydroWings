# Project-HydroWings

**Projektteilnehmer:** Ender Sisman & Philipp Locher  
**Unity-Version:** Unity 6.3 LTS 6000.3.17f1 
**Repository:** [https://github.com/PenguPixel/Project-HydroWings]  

---

### Allgemeine Informationen und kurze Beschreibung zum Projekt
**Project-HydroWings** ist ein stylischer Side-Scrolling Shoot 'em up (Shmup) 

Der Spieler steuert ein bewaffnetes Flugtier (wie den Delfin "DolphWing" oder den Pinguin "PenguWing"), um eine von der übermächtigen Zuckerindustrie bedrohte Welt zu verteidigen. Das Spiel kombiniert klassisches Arcade-Gameplay mit einer taktischen Ressourcen-Mechanik: Um Wasser-Projektile zu verschießen, muss der Wasservorrat aktiv verwaltet werden. Ist der Vorrat erschöpft, taucht der Spieler unter die Wasseroberfläche ab, um nachzuladen.

**Features & Kernmechaniken:**
- **Taktische Wasser- & Tauchmechanik:** Wechsel zwischen Luftkampf und Abtauchen zum Auffüllen der Munition.
- **Upgrade-Progression:** Aufrüstbare Attribute zwischen den Leveln.
- **Dynamische Gegner-Wellen:** Gegnervarianten (Kamikaze-Puffins, Harshmallows) mit Bewegung entlang von Unity-Splines.
- **Boss & Hindernisse:** Vielseitige Attack Patterns im finalen Bosskampf und Umweltgefahren in bonbonbuntem Comic-Look.

---

### Hinweise zum Starten des Projekts
1. **Ausführung im Editor:** Das Projekt muss zwingend aus der Szene `TitleScreen` heraus gestartet werden. Ab dort ist das Spiel über das In-Game-Menü vollständig steuerbar.
2. **Fertiges Build (Windows):** In der eCampus-Abgabe befindet sich der Link zu einem aktuellen Windows-Build (.zip) in der Cloud.

**Steuerung:**
- **Bewegung:** WASD / Pfeiltasten
- **Schießen:** Linksklick
- **Pause / Menü:** per Klick auf Pause Button im Game UI

---

### Wahlpflichtpunkte PRG
- **Mehrere Level:** Level-Progression mit steigendem Schwierigkeitsgrad und Szenenwechsel.
- **Boss-Gegner:** Eigenständiger Bossfight mit komplexen Phasen und Skript-Logik.
- **Extramanöver / Collectibles:** In-Level-Heal-Items zur Wiederherstellung der Trefferpunkte.
- **Komplexere Gegner-KI:** Zufallsbasierte Pfadzuweisung auf Unity-Splines, unterschiedliche Attack-Patterns und Ad-Spawns während des Bosskampfes.
- **Object Pooling:** Performantes `UnityEngine.Pool`-System für Projektile und Gegner-Prefabs.

---

### Wahlpflichtpunkte ALD
- **Leveldesign & Post-Processing:** Parallax-Scrolling über mehrere Hintergrundebenen sowie stilisierter Unterwasser-Effekt via Post-Processing / Shaders.
- **Custom Models & Visuals:** Integration verschiedener 3D-Modelle für Charaktere, Gegner und Obstacles.
- **Spezielle Shader:** Angepasste und modifizierte Shader aus dem Unity Asset Store sowie eigene Shader Graph Implementierungen.
- **Eigene Audio-Komposition:** Eigens komponierter Soundtrack und Sounddesign.
- **Custom VFX / Partikelsysteme:** Eigenes Partikelsystem für Wassertropfen-Spuren bei abgefeuerten Projektilen.
- **Custom Shader:** Eigener Shader für Outline Materials um Cartoon Look zu erzielen.

---

### Liste externer Assets

#### AQUIS - Water Toon Shader  
- **Quelle:** Unity Asset Store (AureDevGames)  
- **Verwendung:** Stylized Wasser-Shader für die `WaterSurface`.  
- **Modifikation:** Origin-Berechnung auf die Objekt-Transformation angepasst, um flüssiges Scrolling zu ermöglichen.

#### War FX  
- **Quelle:** Unity Asset Store (Jean Moreno)  
- **Verwendung:** Explosions-Partikeleffekt bei Treffern / Zerstörung der Gegner.  
- **Modifikation:** Form, Skalierung, Partikelmenge und Lebensdauer an den Comic-Stil angepasst.

#### Sweet LandGUI  
- **Quelle:** Unity Asset Store (Persefida)  
- **Verwendung:** UI-Buttons für das Pause- und Einstellungsmenü.

#### FREE CARTOON PARALLAX 2D BACKGROUNDS  
- **Quelle:** [CraftPix.net](https://craftpix.net/freebies/free-cartoon-parallax-2d-backgrounds/)  
- **Verwendung:** Hintergrund-Ebenen für Level- und Bossszenen.

#### Food Kit  
- **Quelle:** [Kenney.nl](https://kenney.nl/assets/food-kit) (Kenney)  
- **Verwendung:** Süßigkeiten-FBX-Modelle für Hindernisse (Obstacle-Prefabs).
