# Luca Rougefort / Astrify 
17 avril 2025
– PlayerPrefManager – 
V1

---

## Gestionnaire de PlayerPrefs pour Unity

Un outil tout-en-un pour gérer facilement les `PlayerPrefs` dans vos projets Unity, directement depuis l’inspecteur grâce à `NaughtyAttributes`.

## PlayerPrefManager for Unity – Easy Preference Management

A complete all-in-one tool to manage your PlayerPrefs easily in Unity, directly from the Inspector using NaughtyAttributes.


### 🎯 Objectif :
Faciliter la visualisation, l’ajout, la suppression et la modification des préférences utilisateur (`PlayerPrefs`) de manière claire, sécurisée et typée.

### Goal : 

Make managing PlayerPrefs faster, clearer and more secure, with support for value types and Inspector interaction.

---

### ✅ Fonctionnalités :
- Sauvegarde automatique de valeurs `int`, `float`, `string`, `DateTime`
- Détection et chiffrement base64 des valeurs
- Gestion des valeurs par défaut (et bouton de reset)
- Export JSON complet de toutes les préférences
- Interface dans l’Inspector avec des boutons (`NaughtyAttributes`)
- Méthodes statiques accessibles depuis n’importe quel script

### Features : 
- Supports int, float, string, DateTime
- Automatic encryption using base64
- Default values & reset button
- Full JSON export of all saved prefs
- Editor integration using NaughtyAttributes buttons
- Static helper methods to use in any script

---

### 🛠️ Installation :
1. Ajouter le fichier `PlayerPrefManager.cs` dans votre dossier `Assets/Scripts/`
2. Installer le package [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)
3. Créer un GameObject dans la scène (nommé par ex. `PlayerPrefsManager`)
4. Ajouter le script `PlayerPrefManager` en tant que composant
5. Jouer la scène → l’outil est utilisable via l’Inspector

### Installation
1. Add PlayerPrefManager.cs to your project (e.g. Assets/Scripts/)
2. Install NaughtyAttributes (https://github.com/dbrizov/NaughtyAttributes)
3 Create an empty GameObject in the scene
4. Attach the PlayerPrefManager script as a component
5. Enter Play mode → manage your prefs via the Inspector

---

### 🎮 Utilisation dans l’Inspector :
- `Dropdown` pour sélectionner une clé existante
- Bouton `Afficher Valeur` → Affiche la valeur dans la console
- Bouton `Supprimer la clé` → Supprime la clé sélectionnée
- Champs pour ajouter une nouvelle clé + valeur + type
- Bouton `Ajouter Nouvelle Clé` → Enregistre la nouvelle entrée
- Bouton `Reset Defaults` → Recharge les valeurs par défaut

### Inspector Usage :
- Dropdown to select a saved key
- "Show Value" → logs value to the console
- "Delete Key" → removes the selected key
- Input fields for new key / value / type
- "Add New Key" button to save it
- "Reset Defaults" button to restore defaults

---
