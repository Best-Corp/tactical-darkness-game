# RÉSUMÉ DU PROJET - Tactical Darkness

## CONCEPT DU JEU

**Tactical Darkness** est un battle royale tactique d'horreur 2D vue de haut (comme Among Us) où 10 joueurs se battent dans l'obscurité totale avec seulement un halo lumineux 360° pour voir.

### Mécaniques Clés
- **Obscurité totale** - tout est noir sauf le halo du joueur
- **Mort en 1 balle** - pas de barre de vie
- **Halo lumineux 360°** - révèle ce qui est proche (rayon ~12 unités)
- **Boulets s'arrêtent au bord du halo** - impossible de tirer dans le noir
- **Carte qui rétrécit** par élimination (pas par timer)
- **Objets spéciaux** avec effets de 15 secondes
- **Bots IA** pour remplir les lobbies à 10 joueurs

### Contrôles
- ZQSD / WASD = déplacement
- Souris = viser
- Clic gauche = tirer

---

## STACK TECHNIQUE

| Composant | Technologie |
|-----------|-------------|
| **Moteur** | Unity 6.x (6000.6.2f1) |
| **Langage** | C# |
| **Versioning** | Git + GitHub |
| **Assets** | Kenney Top-Down Shooter |
| **Org GitHub** | Best-Corp |
| **Repo** | https://github.com/Best-Corp/tactical-darkness-game |

---

## UNITY CLI

### Emplacement
```
"C:\Program Files\Unity\Hub\Editor\6000.6.2f1\Editor\Unity.exe"
```

### Commandes Utiles

#### Ouvrir le projet
```powershell
Start-Process "C:\Program Files\Unity\Hub\Editor\6000.6.2f1\Editor\Unity.exe" -ArgumentList "-projectPath C:\Users\BC-USER\tactical-darkness-game"
```

#### Compiler les scripts (batch mode)
```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.6.2f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\BC-USER\tactical-darkness-game" -logFile "C:\Users\BC-USER\unity_log.txt"
```

#### Exécuter une méthode Editor
```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.6.2f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\BC-USER\tactical-darkness-game" -executeMethod PlayerSetup.SetupFullScene -logFile -
```

#### Vérifier les erreurs de compilation
```powershell
Select-String -Path "C:\Users\BC-USER\unity_log.txt" -Pattern "error CS"
```

#### Tuer Unity
```powershell
Get-Process -Name Unity -ErrorAction SilentlyContinue | Stop-Process -Force
```

---

## STRUCTURE DU PROJET

```
C:\Users\BC-USER\tactical-darkness-game\
├── Assets/
│   ├── Kenney/                    # Assets Kenney (593 fichiers)
│   │   ├── PNG/
│   │   │   ├── Survivor 1/        # Sprite joueur
│   │   │   ├── Soldier 1/         # Autres sprites
│   │   │   ├── Zombie 1/
│   │   │   ├── Tiles/             # 500+ tiles pour carte
│   │   │   └── weapon_*.png       # Armes
│   │   ├── Spritesheet/
│   │   ├── Tilesheet/
│   │   └── Vector/
│   ├── Scenes/
│   │   └── MainScene.unity        # Scène principale
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs     # Gestion du jeu
│   │   │   └── CameraFollow.cs    # Caméra suit le joueur
│   │   ├── Player/
│   │   │   ├── PlayerMovement.cs  # ZQSD + rotation souris
│   │   │   ├── LightHalo.cs       # Halo lumineux
│   │   │   └── DarknessOverlay.cs # (non utilisé)
│   │   ├── Combat/
│   │   │   ├── Shooting.cs        # Tir 2D
│   │   │   └── Health.cs          # 1 HP = mort
│   │   ├── Map/
│   │   │   └── MapShrink.cs       # Rétrécissement carte
│   │   └── Editor/
│   │       └── PlayerSetup.cs     # Setup automatique scène
│   └── Resources/
├── ProjectSettings/
└── Packages/
```

---

## ÉTAT ACTUEL

### ✅ Fonctionnel
- Joueur avec sprite Kenney Survivor
- Mouvement ZQSD + rotation souris
- Halo lumineux 360° (Point Light, range 12, intensity 8)
- Obscurité totale (pas de Directional Light)
- Caméra orthographic top-down qui suit le joueur
- Sol avec tiles Kenney
- Murs avec tiles Kenney
- Boîtes aléatoires avec colliders
- Système de tir (raycast 2D)
- Système de santé (1 HP)
- GameManager qui compte les éliminations

### ⚠️ À améliorer
- Les murs/boîtes devraient avoir des sprites Kenney plus variés
- Système d'items (5 spéciaux + 3 simples)
- Bots IA (sound/light-based)
- Rétrécissement de la carte
- Annonceur vocal
- UI (health, kills, timer)
- Multiplayer

---

## FICHIERS IMPORTANTS

### PlayerSetup.cs
Script Editor qui crée toute la scène automatiquement:
- Supprime la Directional Light
- Crée le joueur avec sprite, physics, scripts, halo
- Crée la carte avec tiles Kenney
- Sauvegarde la scène

### GameManager.cs
Singleton qui gère:
- Nombre de joueurs vivants
- Éliminations
- Rétrécissement de la carte

### PlayerMovement.cs
- Rigidbody2D (pas de CharacterController)
- gravityScale = 0
- Rotation vers la souris

### Shooting.cs
- Raycast2D
- Vérifie la distance vs halo radius
- Auto-trouve LightHalo et FirePoint

---

## COMMANDES GIT EN FRANÇA

```bash
git commit -m "Ajout du système d'objets"
git commit -m "Correction du mouvement"
git commit -m "Nouvelle fonctionnalité"
```

---

## NOTES POUR LE PROCHAIN LLM

1. **Le projet est en C#/Unity** (pas Unreal, pas Godot)
2. **La caméra est orthographic** top-down (rotation 90° sur X)
3. **La lumière Point Light** crée le halo 360°
4. **Pas de Directional Light** = obscurité totale
5. **Les sprites Kenney** sont dans Assets/Kenney/PNG/
6. **Unity CLI** fonctionne en batch mode pour compiler/exécuter
7. **Les scripts Editor** dans Assets/Scripts/Editor/ créent la scène
8. **Le repo est sur GitHub**: Best-Corp/tactical-darkness-game
9. **Commits en français**
10. **Le jeu est 2D** vue de haut comme Among Us
