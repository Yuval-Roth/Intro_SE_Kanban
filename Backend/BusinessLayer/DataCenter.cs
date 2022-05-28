﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    //===========================================================================
    //                                DataCenter
    //===========================================================================


    /// <summary>
    /// The class manages a data structure of of <c>User</c>s and <c>Board</c>s. <br/><br/>
    /// <code>Supported operations:</code>
    /// <b>-------------User Related--------------</b>
    /// <list type="bullet">
    /// <item>SearchUser(email)</item>
    /// <item>AddUser(email,password)</item>
    /// <item>RemoveUser(email)</item>
    /// <item>ContainsUser(email)</item>
    /// <item>UserLoggedInStatus(email)</item>
    /// <item>SetLoggedIn(email)</item>
    /// <item>SetLoggedOut(email)</item>
    /// </list>
    /// <b>-------------Boards Related--------------</b>
    /// <list type="bullet">
    /// <item>GetBoardsDataUnit(email)</item>
    /// <item>SearchBoardById(board_id)</item>
    /// <item>AddNewBoard(email,board_title)</item>
    /// <item>RemoveBoard(email,board_title)</item>
    /// <item>RemoveBoard(board_id)</item>
    /// <item>JoinExistingBoard(email,board_id)</item>
    /// <item>LeaveJoinedBoard(email,board_id)</item>
    /// </list>
    /// <br/>
    /// ===================
    /// <br/>
    /// <c>Ⓒ Yuval Roth</c>
    /// <br/>
    /// ===================
    /// </summary>
    public class DataCenter
    {


        /*
         
            Reminder: add to the design RemoveBoard(id), LeaveJoinedBoard(email,id), LoadData()
         
         */


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Backend\\BusinessLayer\\DataCenter.cs");

        private struct DataUnit
        {
            public User User { get; init; }
            public BoardsDataUnit BoardsDataUnit { get; init; }
        }
        public struct BoardsDataUnit
        {
            public LinkedList<Board> MyBoards { get; init; }
            public LinkedList<Board> JoinedBoards { get; init; }
        }
        private readonly AVLTree<string, DataUnit> UsersAndBoardsTree;
        private readonly AVLTree<int, Board> OnlyBoardsTree;
        private readonly HashSet<string> loggedIn;
        private int nextBoardID;

        public DataCenter()
        {
            UsersAndBoardsTree = new();
            OnlyBoardsTree = new();
            loggedIn = new();
            //LoadData();
        }

        /// <summary>
        /// Auto incrementing counter for boards ID.<br/>
        /// Everytime this method is called, the counter in incremented.
        /// </summary>
        private int GetNextBoardID => nextBoardID++;

        /// <summary>
        /// Searches for a user with the specified email<br/><br/>
        /// <b>Throws</b> <c>UserDoesNotExistException</c> if the user doesn't exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User</returns>
        /// <exception cref="UserDoesNotExistException"></exception>
        public User SearchUser(string email)
        {
            try
            {
                log.Debug("SearchUser() for: " + email);
                User output = UsersAndBoardsTree.GetData(email).User;
                log.Debug("SearchUser() success");
                return output;
            }
            catch (KeyNotFoundException)
            {
                log.Error("SearchUser() failed: '" + email + "' doesn't exist in the system");
                throw new UserDoesNotExistException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }
        /// <summary>
        /// Adds a user to the system
        /// <br/><br/>
        /// <b>Throws</b> <c>ElementAlreadyExists</c> if a user with this email<br/>
        /// already exists in the system
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ElementAlreadyExistsException"></exception>
        /// <returns>The added <c>User</c></returns>
        public User AddUser(string email, string password)
        {
            try
            {
                log.Debug("AddUser() for: " + email);
                DataUnit data = UsersAndBoardsTree.Add(email, new DataUnit()
                {
                    User = new User(email, password),
                    BoardsDataUnit = new()
                    {
                        MyBoards = new LinkedList<Board>(),
                        JoinedBoards = new LinkedList<Board>()
                    }
                });
                log.Debug("AddUser() success");
                return data.User;
            }
            catch (DuplicateKeysNotSupported)
            {
                log.Error("AddUser() failed: '" + email + "' already exists");
                throw new ElementAlreadyExistsException("A user with the email '" +
                    email + "' already exists in the system");
            }
        }

        /// <summary>
        /// Removes the user with the specified email from the system
        /// <br/><br/>
        /// <b>Throws</b> <c>UserDoesNotExistException</c> if the user doesn't exist in the system
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="UserDoesNotExistException"></exception>
        public void RemoveUser(string email)
        {
            try
            {
                log.Debug("RemoveUser() for: " + email);
                UsersAndBoardsTree.Remove(email);
                /*
                    TO DO:
                    Take care of boards of deleted users
                 */
                log.Debug("RemoveUser() success");
            }
            catch (KeyNotFoundException)
            {
                log.Error("RemoveUser() failed: '" + email + "' doesn't exist");
                throw new UserDoesNotExistException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }

        /// <summary>
        /// Gets the user's logged in status
        /// </summary>
        /// <returns><c>true</c> if the user is logged in, <c>false</c>  otherwise</returns>
        /// <param name="email"></param>
        public bool UserLoggedInStatus(string email)
        {
            log.Debug("UserLoggedInStatus() for: " + email);
            return loggedIn.Contains(email);
        }

        /// <summary>
        /// Sets a user's logged in status to true
        /// <br/><br/>
        /// <b>Throws</b> <c>ArgumentException</c> if the user's logged in status is already true
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetLoggedIn(string email)
        {
            if (UserLoggedInStatus(email) == false)
            {
                log.Info(email + " is now logged in");
                loggedIn.Add(email);
            }
            else
            {
                log.Error("SetLoggedIn() failed: '" + email + "' is already logged in");
                throw new ArgumentException("The user with the email '" + email + "' is already logged in");
            }
        }

        /// <summary>
        /// Sets a user's logged in status to false
        /// <br/><br/>
        /// <b>Throws</b> <c>ArgumentException</c> if the user's logged in status is already false
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetLoggedOut(string email)
        {
            if (UserLoggedInStatus(email) == true)
            {
                log.Info(email + " is now logged out");
                loggedIn.Remove(email);
            }
            else
            {
                log.Error("SetLoggedOut() failed: '" + email + "' is not logged in");
                throw new ArgumentException("The user with the email '" + email + "' is not logged in");
            }
        }

        public bool ContainsUser(string email)
        {
            log.Debug("ContainsUser() for: " + email);
            return UsersAndBoardsTree.Contains(email);
        }

        /// <summary>
        /// Gets all the <c>User</c>'s boards data
        /// <br/><br/>
        /// <b>Throws</b> <c>UserDoesNotExistException</c> if the <c>User</c> does not exist<br/>
        /// in the system
        /// </summary>
        /// <returns><see cref="BoardsDataUnit"/></returns>
        /// <exception cref="UserDoesNotExistException"></exception>
        public BoardsDataUnit GetBoardsDataUnit(string email)
        {
            try
            {
                log.Debug("GetBoardsDataUnit() for: " + email);
                DataUnit data = UsersAndBoardsTree.GetData(email);
                log.Debug("GetBoardsDataUnit() success");
                return data.BoardsDataUnit;
            }
            catch (KeyNotFoundException)
            {
                log.Error("GetBoardsDataUnit() failed: '" + email + "' doesn't exist");
                throw new UserDoesNotExistException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }

        /// <summary>
        /// Gets all the <c>User</c>'s <c>Board</c>s
        /// <br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if the <c>User</c> does not exist<br/>
        /// in the system
        /// </summary>
        /// <returns><see cref="Board"/></returns>
        /// <exception cref="NoSuchElementException"></exception>
        public Board SearchBoardById(int id)
        {
            try
            {
                log.Debug("SearchBoardById() for: " + id);
                Board output = OnlyBoardsTree.GetData(id);
                log.Debug("SearchBoardById() success");
                return output;
            }
            catch (KeyNotFoundException)
            {
                log.Error("SearchBoardById() failed: board number '" + id + "' doesn't exist");
                throw new NoSuchElementException("Board number '" + id + "' doesn't exist");
            }
        }


        /// <summary>
        /// Joins a user to an existing board.<br/><br/>
        /// <b>Throws</b> <c>ElementAlreadyExistsException</c> if the user is already joined on the board<br/>
        /// <b>Throws</b> <c>UserDoesNotExistException</c> if the user doesn't exist in the system<br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if a board with that id doesn't exist<br/>
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <exception cref="ElementAlreadyExistsException"></exception>
        /// <exception cref="UserDoesNotExistException"></exception>
        /// <exception cref="NoSuchElementException"></exception>
        public void JoinExistingBoard(string email, int id)
        {
            try
            {
                log.Debug("JoinExistingBoard() for: " + email + ", " + id);

                LinkedList<Board> joinedBoardList = UsersAndBoardsTree.GetData(email).BoardsDataUnit.JoinedBoards;
                Board boardToJoin = OnlyBoardsTree.GetData(id);

                // Check if the user is joined on the board already
                foreach (Board board in joinedBoardList)
                {
                    if (board.Id == id)
                    {
                        log.Error("JoinExistingBoard() failed: " + email + " is already joined on board nubmer " + id);
                        throw new ElementAlreadyExistsException(email + " is already joined on board nubmer " + id);
                    }
                }
                joinedBoardList.AddLast(boardToJoin);
                log.Debug("JoinExistingBoard() success");
            }
            catch (KeyNotFoundException)
            {
                if (UsersAndBoardsTree.Contains(email) == false)
                {
                    log.Error("JoinExistingBoard() failed: '" + email + "' doesn't exist");
                    throw new UserDoesNotExistException("A user with the email '" +
                        email + "' doesn't exist in the system");
                }
                if (OnlyBoardsTree.Contains(id) == false)
                {
                    log.Error("JoinExistingBoard() failed: board number " + id + "doesn't exist");
                    throw new NoSuchElementException("Board number " + id + "doesn't exist");
                }
            }  
        }

        public void LeaveJoinedBoard(string email, int id) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a <c>Board</c> to the <c>User</c>.
        ///<br/><br/>
        /// <b>Throws</b> <c>ElementAlreadyExistsException</c> if a <c>Board</c> with that title already exists <br/>
        /// for the <c>User</c><br/><br/>
        /// <b>Throws</b> <c>UserDoesNotExistException</c> if the <c>User</c> doesn't exist <br/>
        /// in the system
        /// </summary>
        /// <returns>The <c>Board</c> that was added</returns>
        /// <exception cref="ElementAlreadyExistsException"></exception>
        /// <exception cref="UserDoesNotExistException"></exception>
        public Board AddNewBoard(string email, string title)
        {
            try
            {
                log.Debug("AddBoard() for: " + email + ", " + title);

                // Fetch the user's boards
                LinkedList<Board> myBoardList = UsersAndBoardsTree.GetData(email).BoardsDataUnit.MyBoards;

                // Check if there's a board with that title already
                foreach (Board board in myBoardList)
                {
                    if (board.Title == title)
                    {
                        log.Error("AddBoard() failed: board '" + title + "' already exists for " + email);
                        throw new ElementAlreadyExistsException("A board titled " +
                                title + " already exists for the user with the email " + email);
                    }
                }

                // Add a new board and return it
                Board newBoard = new(title, GetNextBoardID);
                OnlyBoardsTree.Add(newBoard.Id, newBoard);
                myBoardList.AddLast(newBoard);
                log.Debug("AddBoard() success");
                return newBoard;
            }
            catch (DuplicateKeysNotSupported)
            {
                log.Fatal("BoardIDCounter is out of sync");
                throw new DataMisalignedException("BoardIDCounter is out of sync");
            }
            catch (KeyNotFoundException)
            {
                log.Error("AddBoard() failed: '" + email + "' doesn't exist");
                throw new UserDoesNotExistException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }

        public void RemoveBoard(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a <c>User</c>'s <c>Board</c>
        /// <br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if a <c>Board</c> with that title <br/>
        /// doesn't exist for the user<br/><br/>
        /// <b>Throws</b> <c>UserDoesNotExistException</c> if the <c>User</c> doesn't exist <br/>
        /// in the system
        /// </summary>
        /// <exception cref="NoSuchElementException"></exception>
        /// <exception cref="UserDoesNotExistException"></exception>
        public void RemoveBoard(string email, string title)
        {
            /*
            TO DO:                
            update to current requirements:

            1) add deletion by id number
            2) remove pointers from everywhere after deletion
            3) remove from OnlyBoardsTree as well
             */

            try
            {
                log.Debug("RemoveBoard() for: " + email + ", " + title);
                bool found = false;

                // Fetch the user's boards
                LinkedList<Board> boardList = UsersAndBoardsTree.GetData(email).BoardsDataUnit.MyBoards;

                // Search for the specific board
                foreach (Board board in boardList)
                {
                    if (board.Title == title)
                    {
                        boardList.Remove(board);
                        found = true;
                        log.Debug("RemoveBoard() success");
                        break;
                    }
                }

                // didn't find a board by that name
                if (!found)
                {
                    log.Error("RemoveBoard() failed: board '" + title + "' doesn't exist for " + email);
                    throw new NoSuchElementException("A board titled '" +
                                    title + "' doesn't exists for the user with the email " + email);
                }
            }
            catch (KeyNotFoundException)
            {
                log.Error("AddBoard() failed: '" + email + "' doesn't exist");
                throw new UserDoesNotExistException("A user with the email '" +
                    email + "' doesn't exist in the system");

            }
        }

        private void LoadData()
        {
            throw new NotImplementedException();
        }

    }
}