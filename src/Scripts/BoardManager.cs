using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    /*
    TODO:
    Fix allowing players to place on a filled column. The array won't count it, but the object will still run and they will loose turn.
    Add background image
    Add background music
    Add sound effects (win, start, place chip)
     */
    private int[,] board;
   // private int WIDTH = 32 + 6 + 23;
    private int  numColumn, numRow, currentPlayer; //how long is the chip, the distance between slots, center of slot
    public GameObject menuButton, overText, winnerText;
    void Start()
    {
        board = new int[6, 7];
        numColumn = 7;
        numRow = 6;
        currentPlayer = 1;
    }

    public bool isTurn(int id)
    {
        if (id == currentPlayer)
            return true;
        return false;
    }

    void nextTurn()
    {
        if (currentPlayer == 1)
            currentPlayer++;
        else
            currentPlayer--;
    }

    //Add chip to colum, then find the row. Then check if connect four.
    public void addChip(Chip c)
    {
        int row, column = c.column;
        for (row = 0; row < numRow; row++)
            if (board[row, column] == 0)
                break;
        if (row == numRow)
        {
            Debug.LogError("Cant place chip @ " + row + "," + column + "!");
            return;
        }
        c.row = row;
        board[row, column] = c.owner;
        Debug.Log("ADD: Player " + c.owner + " @ " + row + "," + column);
        check(row, column, c.owner);
    }

    void check(int row, int column, int player)
    {
        //row
        if (checkRow(row, player))
            win(player, "row");
        //column
        else if (checkColumn(column, player))
            win(player, "column");
        //diag
        else if (checkDiag(row, column, player))
            win(player, "diag");
        else
            nextTurn();
    }

    private bool checkRow(int row, int player)
    {
        int count = 0;
        for (int col = 0; col < numColumn; col++)
        {
            if (count == 4)
                return true;
            if (board[row, col] == player)
                count++;
            else
                count = 0;
        }
        return false;
    }

    private bool checkColumn(int column, int player)
    {
        int count = 0;
        for (int row = 0; row < numRow; row++)
        {
            if (count == 4)
                return true;
            if (board[row, column] == player)
                count++;
            else
                count = 0;
        }
        return false;
    }

    private bool checkDiag(int row, int column, int player)
    {
        // From Left Down to Right Up
        int count = 0, c;
        //Up and Right
        for (int r = row; r < numRow; r++)
        {
            c = column + (r - row);
            if (c >= numColumn)
                break;
            if (board[r, c] == player)
                count++; 
            else
                break;
        }
        //Down and Left
        for (int r = row - 1; r >= 0; r--) //offset: otherwise it will count the same chip (the one just placed) twice
        {
            c = column - (row - r);
            if (c < 0)
                break;
            if (board[r, c] == player)
                count++;
            else
                break;
        }
        if (count >= 4)
            return true;

        //From Down Right to Up Left
        count = 0;
        //Down and Right
        for (int r = row; r >= 0; r--)
        {
            c = column + (row - r);
            if (c >= numColumn)
                break;
            if (board[r, c] == player)
                count++;
            else
                break;
        }
        //Up and Left
        for (int r = row + 1; r < numRow; r++)
        {
            c = column - (r - row);
            if (c < 0)
                break;
            if (board[r, c] == player)
                count++;
            else
                break;
        }
        if (count >= 4)
            return true;
        return false;
    }

    private void win(int player, string how)
    {
        Debug.Log("GAME OVER! Player " + player + " has won via " + how);
        currentPlayer = 0; // means it is nobodys turn
        menuButton.SetActive(true);
        overText.SetActive(true);
        GameObject.FindGameObjectWithTag("TurnText").GetComponent<Text>().text = "";
        if (player == GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player)
            winnerText.GetComponent<Text>().text = "You Win!";
        else
            winnerText.GetComponent<Text>().text = "You Lose!";
    }
}
