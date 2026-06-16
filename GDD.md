# Game Design Document (GDD) - Project HydroWings

## 1. High Concept & Core Loop
### 1.1 Das High Concept (Die Vision)
Wir kämpfen mit mit Flugtieren in Jet Form gegen Zuckerwesen die von der Zuckerindustrie (Endboss) geschickt wurden um uns ungesund zu machen.
Geschossen wird mit Wasser um den ganzen Zucker aufzulösen.
> 

### 1.2 Kernmerkmale (Key Features)
Gewaltfreies ShootEmUp Game für Kinder und Jugendliche mit Relitätbezug auf negative Auswirkungen von Zucker auf die Gesundheit.

* **Feature 1: Mit Wasser auf Zucker-Gegner schießen um sie zu besiegen ** 
* **Feature 2:** 
* **Feature 3:** 
>

### 1.3 Die Gameplay-Schleife (Core Loop)
*Welche fortlaufenden Aktionen führt der Spieler primär aus? (Sekunden- / Minuten-basiert)*
1. **Aktion A (Moment-to-Moment):** 2. **Aktion B (Herausforderung):** 3. **Aktion C (Belohnung/Fortschritt):** *Visuelle Kette:*
> [Aktion A] ➔ [Aktion B] ➔ [Aktion C] ➔ Zurück zu [Aktion A]

### 1.4 Zielgruppe & Plattformen
* **Primäre Zielgruppe:** * **Plattformen:** Unity 6.3 LTS (PC)

### 1.5 Visueller Stil & Atmosphäre
*Kurze Notizen zum gewünschten Grafikstil (z. B. 3D-Assets, stilisierter Wasser-Look) und der Stimmung.*
> 

---

## 2. Steuerung & Movement (Bereits implementiert)
* [cite_start]**Input-System:** Unity 6 New Input System (Gekoppeltes System)[cite: 90, 122, 178].
* [cite_start]**Bewegung:** Unabhängig von der Objektrotation auf den globalen Achsen (`Vector3.right` und `Vector3.up`), um die Flugbahn auf der 2D-Ebene stabil zu halten[cite: 140, 195, 196].
* [cite_start]**Neigung (Pitch):** Automatische optische Drehung um die X-Achse bei vertikalem Input (begrenzt via `Mathf.Clamp` auf $\pm25^\circ$)[cite: 125, 144, 175]. [cite_start]Automatische Rückstellung auf $0^\circ$ via `Mathf.MoveTowards` bei Input-Stop[cite: 200, 209].

---

## 3. Kamerasystem & Parallax (Bereits implementiert)
* [cite_start]**CameraController:** Die Kamera scrollt kontinuierlich autonom nach rechts (`_currentX`)[cite: 546, 569]. Die vertikale Position ($Y$) folgt der Spieler-Position weich gedämpft über `Mathf.SmoothDamp` (`smoothTime`)[cite: 323, 558, 568].
* [cite_start]**ParallaxController:** Berechnet die relative Verschiebung der visuellen Hintergrundebenen proportional zur veränderten X-Position der Kamera, um Tiefe zu erzeugen[cite: 548].

---

## 4. Waffensystem & Projektile (Bereits implementiert)
* [cite_start]**Architektur:** Strikte Trennung zwischen Steuerung (`WeaponController` auf dem Hauptobjekt) und den Abschusspunkten (`WeaponPoint` als Kindobjekte, z.B. Schnabel)[cite: 439, 442, 472].
* [cite_start]**Datenverwaltung:** `ProjectileStats` (ScriptableObject) speichert Schadens- und Geschwindigkeitswerte sowie eine Liste erlaubter Prefabs[cite: 369, 465].
* [cite_start]**Mesh-Auswahl:** Die Methode `GetRandomProjectilePrefab()` wählt per `UnityEngine.Random.Range` ein zufälliges Prefab aus dem ScriptableObject (z.B. verschiedene Wasser-Meshes)[cite: 352, 412].
* [cite_start]**Projektil-Logik:** `Projectile.cs` sitzt auf den Prefabs, verwaltet die Lebensdauer und bewegt das Objekt rein basierend auf den zugewiesenen Stats entlang der globalen X-Achse[cite: 419, 424].
* [cite_start]**Power-up-Erweiterung:** Über `RegisterWeaponPoint()` und `UnregisterWeaponPoint()` können sich zusätzliche Geschütze dynamisch im `WeaponController` an- und abmelden[cite: 457, 507].