using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class GameMode{
    
    public int[] score = new int[4];    // Keeps track of each player/team's score (Eggs, Kills, Roost Points)
    public int MaxScore;
    public ScoreMode SM;                // Time, Lives, Score
    public GameType GT;                 // Battle, TeamBattle, CaptureTheEgg, TeamCaptureTheEgg, KingOfTheRoost
    public float maxTime;               // How long the match runs for (in seconds)
    public int[] Lives = new int[4];   //Lives for each player.
    
    private StatTable stats;
    private MultiplayerController control;


    public void StartGame(GameType gameType, ScoreMode scoreType, bool[] players, int gameLength = 0, int maxScore = 0, int lives = 0)
    {
        control = GameObject.FindObjectOfType<MultiplayerController>();

        GT = gameType;
        SM = scoreType;

        if (gameLength > 0)
        {
            float startTime = Time.time;
            maxTime = startTime + gameLength*60;
        }

        if (maxScore > 0)
        {
            MaxScore = maxScore;
        }

        if (lives > 0)
        {
            for( int i = 0; i < players.Length; i++)
            {
                if(players[i])
                {
                    Lives[i] = lives;
                }
            }
        }
        
        stats = new StatTable(players);
    }

    /// <summary>
    /// Lists the player/team that has won
    /// </summary>
    /// <returns>-1 for NotEnded, 4 for Draw</returns>
    public int CheckGameEnd()
    {
        int victor = -1;
        switch (GT)
        {
            case GameType.Battle:
                victor = CheckBattle();
                break;
            case GameType.TeamBattle:
                victor = CheckTeamBattle();
                break;
            case GameType.CaptureTheEgg:
                victor = CheckCaptureTheEgg();
                break;
            case GameType.TeamCaptureTheEgg:
                victor = CheckTeamCaptureTheEgg();
                break;
            case GameType.KingOfTheRoost:
                victor = CheckKingOfTheRoost();
                break;
        }
        return victor;
    }

    private int CheckBattle()
    {
        int winner;

        if (SM == ScoreMode.Time)
        {
            winner = CheckGameTime();
        }
        else
        {
            winner = CheckLives();
        }
        return winner;
    }

    private int CheckTeamBattle()
    {
        int winner;

        if (SM == ScoreMode.Time)
        {
            winner = CheckGameTime();
        }
        else
        {
            winner = CheckLives();
        }
        return winner;
    }

    private int CheckCaptureTheEgg()
    {
        int winner;

        if (SM == ScoreMode.Time)
        {
            winner = CheckGameTime();
        }
        else
        {
            winner = CheckScore();
        }
        return winner;
    }

    private int CheckTeamCaptureTheEgg()
    {
        int winner;

        if (SM == ScoreMode.Time)
        {
            winner = CheckGameTime();
        }
        else
        {
            winner = CheckScore();
        }
        return winner;
    }

    private int CheckKingOfTheRoost()
    {
        int winner;

        if (SM == ScoreMode.Time)
        {
            winner = CheckGameTime();
        }
        else
        {
            winner = CheckScore();
        }
        return winner;
    }

    private int CheckGameTime()
    {
        if (Time.time >= maxTime)
            return -1;
        else
        {
            int maxNo = 0;
            int playerNo = -1;
            foreach(StatColumn c in stats.playerstats)
            {
                if (c.score == maxNo)
                {
                    playerNo = -1;
                }
                else if (c.score > maxNo)
                {
                    maxNo = c.score;
                    playerNo = c.playerNumber;
                }
            }
            return 0;
        }
    }

    private int CheckLives()
    {
        int playerNo = -1;
        int alivePlayers = 0;
        foreach (StatColumn c in stats.playerstats)
        {
            int lives = Lives[c.playerNumber - 1];
            if(lives > 0)
            {
                alivePlayers++;
                playerNo = c.playerNumber;
            }
            if (alivePlayers > 1)
                break;
        }
        if (alivePlayers == 1)
            return playerNo;
        else if (alivePlayers == 0)
            return 4;
        else
            return -1;
    }

    private int CheckScore()
    {
        foreach (StatColumn c in stats.playerstats)
        {
            if (c.score >= MaxScore)
            {
                return c.playerNumber;
            }
        }
        return -1;
    }

    public void UpdateScore(int PlayerNumber, int scoreAmount = 1)
    {
        foreach (StatColumn c in stats.playerstats)
        {
            if (c.playerNumber == PlayerNumber)
            {
                c.score += scoreAmount;
            }
        }
        CheckScore();
    }

    public void UpdateStats(int PlayerNumber, KillType type)
    {
       foreach(StatColumn c in stats.playerstats)
        {
            if(c.playerNumber == PlayerNumber)
            {
                switch(type)
                {
                    case KillType.Cannon:
                        {
                            c.cannonDeaths++;
                            break;
                        }
                    case KillType.Fan:
                        {
                            c.fanDeaths++;
                            break;
                        }
                    case KillType.Lava:
                        {
                            c.lavaDeaths++;
                            break;
                        }
                    case KillType.Piston:
                        {
                            c.pistonDeaths++;
                            break;
                        }
                    case KillType.SpikyBush:
                        {
                            c.spikyBushDeaths++;
                            break;
                        }
                    case KillType.Suicide:
                        {
                            c.suicides++;
                            break;
                        }
                    case KillType.P1:
                        {
                            c.p1Deaths++;
                            if (GT == GameType.Battle || GT == GameType.TeamBattle)
                            {
                                UpdateScore(1, 1);
                            }
                            break;
                        }
                    case KillType.P2:
                        {
                            c.p1Deaths++;
                            if (GT == GameType.Battle || GT == GameType.TeamBattle)
                            {
                                UpdateScore(2, 1);
                            }
                            break;
                        }
                    case KillType.P3:
                        {
                            c.p1Deaths++;
                            if (GT == GameType.Battle || GT == GameType.TeamBattle)
                            {
                                UpdateScore(3, 1);
                            }
                            break;
                        }
                    case KillType.P4:
                        {
                            c.p1Deaths++;
                            if (GT == GameType.Battle || GT == GameType.TeamBattle)
                            {
                                UpdateScore(4, 1);
                            }
                            break;
                        }
                }
            }
        }
    }
}

public class StatTable
{
    public List<StatColumn> playerstats;
    public StatTable(bool[] players)
    {
        int i = 0;
        foreach(bool p in players)
        {
            if(p)
            {
                playerstats.Add(new StatColumn(i));
            }
            i++;
        }
    }
}

public class StatColumn
{
    // Stats
    public int playerNumber;
    public int score;
    public int kos;
    public int deaths;
    public int suicides;
    public int pistonDeaths;
    public int fanDeaths;
    public int cannonDeaths;
    public int lavaDeaths;
    public int spikyBushDeaths;
    public int p1Deaths;
    public int p2Deaths;
    public int p3Deaths;
    public int p4Deaths;

    public StatColumn(int number)
    {
        playerNumber = number;
    }
}

public enum KillType
{
    Suicide,
    Piston, 
    Fan,
    Cannon,
    Lava,
    SpikyBush,
    P1,
    P2,
    P3,
    P4
}