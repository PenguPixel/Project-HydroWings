# Game Design Document (GDD) - Project HydroWings

## 1. High Concept & Core Loop
### 1.1 Das High Concept (Die Vision)
Wir kämpfen mit mit Flugtieren in Jet Form gegen Zuckerwesen die von der Zuckerindustrie (Endboss) geschickt wurden 
um uns ungesund zu machen.
Geschossen wird mit Wasser um den ganzen Zucker aufzulösen.
> 

### 1.2 Kernmerkmale (Key Features)
Gewaltfreies ShootEmUp Game für Kinder und Jugendliche mit Relitätbezug auf negative Auswirkungen von Zucker 
auf die Gesundheit.

* Feature 1: Mit Wasser auf Zucker-Gegner schießen um sie zu besiegen  
* Feature 2: Wasser muss nachgeladen werden, dafür muss man unter Wasser
* Feature 3: Es gibt Power Ups die zusätzliche/stärkere Geschosse ermöglichen oder den Wasservorrat vergrößern
>

### 1.3 Die Gameplay-Schleife (Core Loop)
Der Spieler steuert den Character und feuert dabei auf Gegner die wiederrum selbst auf den Character feuern 
und deren Projektilen man ausweichen muss. Außerdem muss der Spieler seine Wassermunition im Auge behalten um für 
intensive Phasen gerüstet zu sein.

Es gibt Hindernisse im Level denen ausgewichen werden muss, teilweise wird das nur unter Wasser funktionieren.

1. Aktion A (Moment-to-Moment):			Schießen, Gegner besiegen, ausweichen
2. Aktion B (Herausforderung):			Starke Power Ups aufsammeln und Boss Gegner am Ende des Levels besiegen
3. Aktion C (Belohnung/Fortschritt): 	Durch Abschüsse Punkte sammeln für permanente Upgrades um stärker in Level zu starten 
										-> Metaprogression

> [Aktion A] ➔ [Aktion B] ➔ [Aktion C] ➔ Zurück zu [Aktion A]

### 1.4 Zielgruppe & Plattformen
* Primäre Zielgruppe:	Kinder und Jugendliche ab 12 Jahren 
* Plattformen:			PC, Mobile, Web

### 1.5 Visueller Stil & Atmosphäre
Der Stil ist eher Comichaft gehalten mit stilisierten Formen, Oberflächen und Effekten. 
Die Anmutung ist bund und im Candy Look.
> 

---

## 2. Steuerung & Movement (Bereits implementiert)
* **Input-System:**			Unity 6 New Input System (Gekoppeltes System).
* **Bewegung:**				Unabhängig von der Objektrotation auf den globalen Achsen (`Vector3.right` und `Vector3.up`), um die Flugbahn auf der 2D-Ebene stabil zu halten.
* **Neigung (Pitch):** 		Automatische optische Drehung um die X-Achse bei vertikalem Input (begrenzt via `Mathf.Clamp`. Automatische Rückstellung auf `Mathf.MoveTowards` bei Input-Stop.

---

## 3. Kamerasystem & Parallax (Bereits implementiert)
* **CameraController:** 	Die Kamera scrollt kontinuierlich autonom nach rechts (`_currentX`). Die vertikale Position folgt der Spieler-Position weich gedämpft über `Mathf.SmoothDamp` (`smoothTime`).
* **ParallaxController:** 	Berechnet die relative Verschiebung der visuellen Hintergrundebenen proportional zur veränderten X-Position der Kamera, um Tiefe zu erzeugen.

---

## 4. Waffensystem & Projektile (Bereits implementiert)
* **Architektur:** 			Strikte Trennung zwischen Steuerung (`WeaponController` auf dem Hauptobjekt) und den Abschusspunkten (`WeaponPoint` als Kindobjekte, z.B. Schnabel).
* **Datenverwaltung:** 		`ProjectileStats` (ScriptableObject) speichert Schadens- und Geschwindigkeitswerte sowie eine Liste erlaubter Prefabs.
* **Mesh-Auswahl:** 		Die Methode `GetRandomProjectilePrefab()` wählt per `UnityEngine.Random.Range` ein zufälliges Prefab aus dem ScriptableObject (z.B. verschiedene Wasser-Meshes).
* **Projektil-Logik:** 		`Projectile.cs` sitzt auf den Prefabs, verwaltet die Lebensdauer und bewegt das Objekt rein basierend auf den zugewiesenen Stats entlang der globalen X-Achse.
* **Power-up-Erweiterung:**	Über `RegisterWeaponPoint()` und `UnregisterWeaponPoint()` können sich zusätzliche Geschütze dynamisch im `WeaponController` an- und abmelden.