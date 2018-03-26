# EdwardBellaJacob

## Objectifs du projet

L'objectif du projet est d'implémenter une intelligence artificielle capable de jouer au jeu VampireVSWerewolves. Ce projet a été réalisé pour le cours d'Intelligence Artificielle de l'Ecole Centrale Paris au cours de l'année 2017-2018.

## Organisation du code

### Dossier `Trame`

Le dossier `Trame` contient tout ce qui concerne l’envoi et la réception des différentes trames entre le serveur et le joueur. Il est divisé entre deux sous-dossiers : `PlayerServer` et `ServerPlayer`.

Le dossier `PlayerServer` contient :

- Une classe mère `BaseServerPlayerTrame` qui gère les deux trames que le joueur envoie au serveur
- Deux classes filles `MOVTrame` et `NMETrame` qui implémentent la trame de déplacement (MOV) et la trame de nom (NME).

Le dossier `ServerPlayer` contient :

- Une interface `IDecodable`
- Un ensemble de classes `XYZDecoder` qui implémentent toutes l’interface `IDecodable`, et qui permettent chacune de décoder une trame de type XYZ.
- Une classe `PlayerServerTrame`, qui va permettre d’attendre la réception d’une trame et qui, en fonction du header, va exécuter le décodeur correspondant.

### Dossier `Rules` :

Il contient plusieurs éléments relatifs à la logique du jeux :

- Une classe `Board` qui encapsule la logique du jeux. C'est ici que l'on update la grille de jeux et que l'on calcule l'ensemble des coups possibles.
- Une classe `Coord` qui implémente les coordonnées. Elle contient deux propriétés x et y (entiers).
- Une classe `Pawn` qui sert à implémenter les groupes de pions sur une case. Elle contient 3 propriétés : les coordonnées de la case (objet `Coord`), son type (type énuméré) et sa quantité (int).
- Une classe `Grid` qui est un wrapper de la collection de pions dont est constitué la classe `Board`. Elle contient donc 1 propriété : la collection de pions (collection d’objets Pawn) et des méthodes pour ajouter et déplacer les pions. Aujourd'hui la collection choisie est un dictionnaire dont la clé est un objet `Coord` et la valeur un objet `Pawn`.
- Une classe `Move` qui sert à représenter la logique de déplacement du jeu. Elle contient les coordonnées du pion que l'on souhaite déplacer, sa quantité ainsi que sa direction (type énuméré).
- Une classe `SplitEumeration` utile pour calculer l'ensemble des coups possibles d'un tour à l'autre.

### Dossier `MinMax` :

Il contient plusieurs éléments relatifs à l'implémentation de l'algorithme MinMax :

- Un fichier `Node` qui impélemente la logique de l'arbre de recherche.
- Une classe `MinMax` qui contient notre implémentation de l'algorithme MinMax.
- Une classe `Heuristic` qui calcule le score du board.
- Une classe `BaseIA` qui est la classe mère de MinMax et qui aurait davantage servie si on avait eu le temps d'implémenter d'autres algorithmes.

### Classe `Client` :

Cette classe permet l’initialisation de la connexion avec le serveur, et l’échange de données avec ce serveur.  C’est dans cette classe que se lance deux IA (l’une lançant un MinMax sans split, l’autre avec split) dans deux threads parallèles, et au bout d’un temps limité il regarde quel IA a abouti à un résultat. L’IA donne la priorité à l’IA qui a fini ses calculs, et sinon il donne la priorité à l’IA avec le meilleur coup actuel.

### Classe `Program` :

C'est ici que la classe client est appelée et lance sa boucle principale.
