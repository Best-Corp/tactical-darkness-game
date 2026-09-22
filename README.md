# Tactical Darkness - Jeu de Combat Tactique

Un battle royale tactique d'horreur où 10 joueurs se battent dans l'obscurité totale avec seulement un halo lumineux pour voir. Fonctionnalités : morts en un coup, carte rétrécissante par éliminations, et objets uniques.

## Mécaniques Principales

- **Visibilité dans l'Obscurité** : Halo lumineux 360° révèle uniquement ce qui est proche
- **Morts en un Coups** : 1 balle = mort instantanée, pas de barre de vie
- **Carte Rétrécissante** : La carte se réduit avec chaque élimination, pas par timer
- **Objets Uniques** : 5 objets spéciaux avec effets de 15 secondes
- **Bots IA** : Remplissent les lobbies à 10 joueurs avec IA basée sur le son/lumière

## Technologies

- Unity 6.x (6000.6.2f1)
- C#
- Git pour le contrôle de version

## Structure du Projet

```
Assets/
  Scripts/
    Core/           - Game mode, game manager, caméra
    Player/         - Mouvement du joueur, halo lumineux
    Combat/         - Tir, système de dégâts
    Items/          - Système d'objets, effets
    Map/            - Rétrécissement, visibilité
  Scenes/           - Niveaux de jeu
  Materials/        - Éclairage, shaders
  Audio/            - Effets sonores
  Prefabs/          - Objets réutilisables
```

## Installation

1. Cloner ce dépôt
2. Ouvrir Unity Hub
3. Ajouter le projet depuis le disque (sélectionner ce dossier)
4. Ouvrir et jouer

## Comment Ouvrir le Projet

### Option 1 : Cloner depuis GitHub
```bash
git clone https://github.com/Best-Corp/tactical-darkness-game.git
```
Puis ouvrir Unity Hub → Ajouter projet depuis le disque → sélectionner le dossier.

### Option 2 : Unity Hub
1. Ouvrir **Unity Hub**
2. Cliquer **Ajouter** → **Ajouter projet depuis le disque**
3. Naviguer vers le dossier cloné
4. Sélectionner le dossier racine

## Prérequis

| Prérequis | Version |
|-----------|---------|
| **Unity** | 6000.6.2f1 (ou similaire 6.x) |
| **Git** | Toute version récente |

## Commandes de Jeu

| Action | Contrôles |
|--------|-----------|
| **Déplacement** | ZQSD / WASD |
| **Viser** | Position de la souris |
| **Tirer** | Clic gauche |

## Équipe

- Chef de Projet
- Programmeurs
- Artistes
- Concepteurs

## Licence

Privé - Tous droits réservés
