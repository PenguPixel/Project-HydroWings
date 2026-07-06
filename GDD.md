# Game Design Document (GDD) - Project HydroWings

## 1. High Concept & Core Loop
### 1.1 Das High Concept (Die Vision)
In "Project HydroWings" kämpfen Spieler mit fliegenden Düsen-Tierwesen gegen bösartige Zucker-Kreaturen, die von der mächtigen Zuckerindustrie (dem Endboss) entsandt wurden, um die Welt ungesund zu machen. Geschossen wird mit reinem Wasser, um die klebrigen Zucker-Gegner restlos aufzulösen.
> 

### 1.2 Kernmerkmale (Key Features)
* **Gewaltfreies Gameplay:** Ein Shoot 'em up speziell für Kinder und Jugendliche mit einem spielerischen Realitätsbezug zu den negativen Auswirkungen von übermäßigem Zuckerkonsum auf die Gesundheit.
* **Wasser als Munition:** Spieler bekämpfen die Zucker-Gegner mit Wasser-Projektilen.
* **Tauch-Nachlademechanik:** Der Wasservorrat ist begrenzt. Um die Waffe nachzuladen, muss das Flugtier aktiv unter die Wasseroberfläche tauchen.
* **Modulare Power-ups:** Einsammelbare Upgrades ermöglichen zusätzliche oder stärkere Geschosse und erweitern den maximalen Wasservorrat.
> 

### 1.3 Die Gameplay-Schleife (Core Loop)
Der Spieler steuert das Flugtier durch den Level, weicht feindlichen Projektilen sowie Hindernissen aus und eliminiert gegnerische Formationen. Ein strategisches Ressourcen-Management ist zwingend erforderlich, da der Wasservorrat für intensive Kampfphasen rechtzeitig unter Wasser aufgefüllt werden muss. Bestimmte Barrieren im Level lassen sich zudem nur durch das Tauchen fehlerfrei passieren.

1. **Aktion A (Moment-to-Moment):** Fliegen, feindlichen Geschossen ausweichen, Hindernisse umfliegen/untertauchen und präzise mit Wasser auf Gegner schießen.
2. **Aktion B (Herausforderung):** Starke Power-ups taktisch einsammeln, das Ressourcen-Management meistern und mächtige Boss-Gegner am Ende der Level bezwingen.
3. **Aktion C (Belohnung/Fortschritt):** Durch Abschüsse Punkte sammeln, um permanente Upgrades freizuschalten, mit denen man stärker in den nächsten Versuch startet (Meta-Progression).

> [Aktion A] ➔ [Aktion B] ➔ [Aktion C] ➔ Zurück zu [Aktion A]

### 1.4 Zielgruppe & Plattformen
* **Primäre Zielgruppe:** Kinder und Jugendliche ab 12 Jahren.
* **Plattformen:** PC, Mobile, Web (Unity 6.3 LTS).

### 1.5 Visueller Stil & Atmosphäre
Der Grafikstil ist in einem farbenfrohen, stilisierten Comic-Look gehalten. Die Spielwelt präsentiert sich in einer bunten "Candy-Optik" mit verspielten Oberflächenformen und lebendigen Partikeleffekten.
> 

---

## 2. Steuerung & Movement (Bereits implementiert)
* **Input-System:** Unity 6 New Input System (Gekoppeltes System).
* **Bewegung:** Unabhängig von der Objektrotation auf den globalen Achsen (`Vector3.right` und `Vector3.up`), um die Flugbahn auf der 2D-Ebene stabil zu halten.
* **Neigung (Pitch):** Automatische optische Drehung um die X-Achse bei vertikalem Input (begrenzt via `Mathf.Clamp`). Automatische Rückstellung auf $0^\circ$ via `Mathf.MoveTowards` bei Input-Stop.

---

## 3. Kamerasystem & Parallax (Bereits implementiert)
* **CameraController:** Die Kamera scrollt kontinuierlich autonom nach rechts (`_currentX`). Die vertikale Position folgt der Spieler-Position weich gedämpft über `Mathf.SmoothDamp` (`smoothTime`).
* **ParallaxController:** Berechnet die relative Verschiebung der visuellen Hintergrundebenen proportional zur veränderten X-Position der Kamera, um Tiefe zu erzeugen.

---

## 4. Waffensystem & Projektile (Bereits implementiert)
* **Architektur:** Strikte Trennung zwischen Steuerung (`WeaponController` auf dem Hauptobjekt) und den Abschusspunkten (`WeaponPoint` als Kindobjekte, z. B. Schnabel).
* **Datenverwaltung:** `ProjectileStats` (ScriptableObject) speichert Schadens- und Geschwindigkeitswerte sowie eine Liste erlaubter Prefabs.
* **Mesh-Auswahl:** Die Methode `GetRandomProjectilePrefab()` wählt per `UnityEngine.Random.Range` ein zufälliges Prefab aus dem ScriptableObject (z. B. verschiedene Wasser-Meshes).
* **Projektil-Logik:** `Projectile.cs` sitzt auf den Prefabs, verwaltet die Lebensdauer und bewegt das Objekt rein basierend auf den zugewiesenen Stats entlang der globalen X-Achse.
* **Power-up-Erweiterung:** Über `RegisterWeaponPoint()` und `UnregisterWeaponPoint()` können sich zusätzliche Geschütze dynamisch im `WeaponController` an- und abmelden.