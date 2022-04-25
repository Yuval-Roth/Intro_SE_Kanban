﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    //===========================================================================
    //                                UserData
    //===========================================================================


    /// <summary>
    /// The class manages a data structure of of <c>User</c>s and their <c>Board</c>s. <br/>
    /// every unique <c>User</c> has his own set of <c>Board</c>s.
    /// <code>Supported operations:</code>
    /// <list type="bullet">SearchUser()</list>
    /// <list type="bullet">AddUser()</list>
    /// <list type="bullet">RemoveUser()</list>
    /// <list type="bullet">GetAllBoards()</list>
    /// <list type="bullet">AddBoard()</list>
    /// <list type="bullet">RemoveBoard()</list>
    /// <br/><br/>
    /// ===================
    /// <br/>
    /// <c>Ⓒ Yuval Roth</c>
    /// <br/>
    /// ===================
    /// </summary>
    public class UserData
    {
        private class DataUnit
        {
            public User User { get; set; }
            public LinkedList<Board> boards { get; set; }
        }
        private BinaryTree<string, DataUnit> tree;

        public UserData()
        {
            tree = new BinaryTree<string, DataUnit>();
        }

        /// <summary>
        /// Searches for a user with the specified email<br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if the user doesn't exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User</returns>
        /// <exception cref="NoSuchElementException"></exception>
        public User SearchUser(string email)
        {
            try
            {
                return tree.GetData(email).User;
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
            
        }

        /// <summary>
        /// Adds a user to the system
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>The added <c>User</c></returns>
        public User AddUser(string email)
        {
            try
            {
                return tree.Add(email, new DataUnit()).User;
            }
            catch (ArgumentException) 
            {
                throw new ArgumentException("A user with the email '" +
                    email + "' already exists in the system");
            }        
        }

        /// <summary>
        /// Removes the user with the specified email from the system
        /// <br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if the user doesn't exist in the system
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="NoSuchElementException"></exception>
        public void RemoveUser(string email) 
        {
            try
            {
                tree.Remove(email);
            }
            catch (NoSuchElementException) 
            {
                throw new NoSuchElementException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }

        /// <summary>
        /// Gets all the <c>User</c>'s <c>Board</c>s
        /// <br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if the <c>User</c> does not exist<br/>
        /// in the system
        /// </summary>
        /// <returns><c>LinkedList</c> of type <c>Board</c></returns>
        /// <exception cref="NoSuchElementException"></exception>
        public LinkedList<Board> GetBoards(string email)
        {
            try
            {
                return tree.GetData(email).boards;
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }

        /// <summary>
        /// Adds a <c>Board</c> to the <c>User</c>.
        ///<br/><br/>
        ///<b>Throws</b> <c>ArgumentException</c> if a <c>Board</c> with that title already exists <br/>
        /// for the <c>User</c><br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if the <c>User</c> doesn't exist <br/>
        /// in the system
        /// </summary>
        /// <returns>The <c>Board</c> that was added</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSuchElementException"></exception>
        public Board AddBoard(string email,string title) 
        {
            try
            {
                LinkedList<Board> boardList = tree.GetData(email).boards;
                foreach (Board board in boardList) 
                {
                    if (board.Title == title)
                        throw new ArgumentException("A board titled " +
                            title + " already exists for the user with the email " + email);                
                }
                Board toReturn = new(title);
                boardList.AddLast(toReturn);
                return toReturn;
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }

        /// <summary>
        /// Removes a <c>User</c>'s <c>Board</c>
        /// <br/><br/>
        /// <b>Throws</b> <c>ArgumentException</c> if a <c>Board</c> with that title <br/>
        /// doesn't exist for the user<br/><br/>
        /// <b>Throws</b> <c>NoSuchElementException</c> if the <c>User</c> doesn't exist <br/>
        /// in the system
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// /// <exception cref="NoSuchElementException"></exception>
        public void RemoveBoard(string email, string title)
        {
            try
            {
                bool found = false;
                LinkedList<Board> boardList = tree.GetData(email).boards;
                foreach (Board board in boardList)
                {
                    if (board.Title == title) 
                    {
                        boardList.Remove(board);
                        found = true;
                        break;
                    }

                }
                if (! found)
                    throw new ArgumentException("A board titled " +
                                title + " doesn't exists for the user with the email " + email);
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException("A user with the email '" +
                    email + "' doesn't exist in the system");
            }
        }
    }
}