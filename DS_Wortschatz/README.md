# DS_WorthSchatz



1. Deutsch

    Ein System zum spielerischen Erlernen deutscher oder serbischer Wörter sowie zum Suchen nach Wörtern und Artikeln von Substantiven, das folgende Aufgaben erfüllen soll:
    
    Oberfläche: graphische Benutzeroberfläche (WPF)
    • Ein Spiel, bei dem es darum geht, die richtigen Artikel eines Wortes zu erraten
    • Zusätzlich können Wörter gesucht werden, das Programm zeigt die Übersetzung und den richtigen Artikel für die Wörter an (optional)
    Mit dem Spiel werden eine Anzahl an Wörtern inklusive Übersetzung mitgeliefert. Der Administrator kann diese Liste jedoch Bearbeiten und erweitern
    Definition von Aufgaben durch den Administrator. Eine Aufgabe ist ein Subset aller Wörter
    • Statistiken pro Aufgabe und Spieler: gelernte Wörter und Artikel und Fehler, Zeitaufwand für das Lernen ...
 	Verwaltung von Benutzern mit verschiedenen Rollen:
	   - Normaler Benutzer kann Lernspiele machen und sieht die Statistiken seiner Lernspiele
	   - Administrator kann Benutzer, Wörter und Aufgaben verwalten. Die Statistiken der Benutzer können angezeigt werden
	• Ein grundlegendes Aufstiegssystem durch Punkte und Erfolge (Medaillen), um das Lernen weiter zu fördern
transalte
 
2. Englisch
  
	A system for playful learning of German or Serbian words, as well as searching for words and articles of nouns, that should fulfill the following tasks:

    Interface: graphical user interface (WPF)
    • A game where the objective is to guess the correct articles of a word.
    • Additionally, words can be searched for, and the program displays the translation and the correct article for the words (optional).
    The game comes with a set number of words including translations. However, the administrator can edit and expand this list.
    Definition of tasks by the administrator. A task is a subset of all words.
    • Statistics per task and player: learned words and articles, mistakes, time spent on learning, etc.
    Management of users with different roles:
        - Regular user: Can participate in learning games and view the statistics of their learning games.
        - Administrator: Can manage users, words, and tasks. The statistics of users can be displayed.
    • A basic progression system through points and achievements (medals) to further promote learning.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Repository](#repository)


## Installation

### Prerequisites

Ensure you have the following software installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

#### Steps

1. **Clone the repository**:
   ```bash
   git clone https://github.com/MicaFica112/DS_Wortschatz
   cd your-repository
2. **Restore the Dependencies**: Explains how to restore the dependencies using `dotnet restore`.
3. **Build the Project**: 
4. **Run the Project**: 
5. **List of Dependencies**: 

     This project uses the following NuGet packages:

  1. PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" 
  2. PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.15"
  3.PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.14"
  4.PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="7.0.15" 
  5.PackageReference Include="Microsoft.Extensions.Configuration" Version="7.0.0" 
  6.PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="7.0.4" 
  7.PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="7.0.0" 
   

   These dependencies are automatically restored when you run `dotnet restore`.

 ## Usage

    ** Login as Admin  
    Username: admin 
    Pass: admin
    
    ** Login as User 
    Username: user 
    Pass: user

 ## Repository

 https://github.com/MicaFica112/DS_Wortschatz
