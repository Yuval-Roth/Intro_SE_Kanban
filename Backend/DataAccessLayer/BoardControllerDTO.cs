﻿
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardControllerDTO
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Backend\\DataAccessLayer\\BoardControllerDTO.cs");

        private SQLExecuter executer;

        public BoardControllerDTO(SQLExecuter executer)
        {
            this.executer = executer;
        }

        public bool AddBoard(int Id,string Title ,string Owner)
        {
            log.Debug($"AddBoard() for {Id}, {Title}, {Owner}");
            return executer.ExecuteWrite ("INSERT into Boards (BoardId, BoardTitle, Owner, BacklogLimit, InprogressLimit, DoneLimit,TaskIDCounter) " +
                                         $"VALUES({Id},'{Title}','{Owner}',-1,-1,-1,0)");
        }
        public bool RemoveBoard(int Id)
        {
            log.Debug($"RemoveBoard() for {Id}");
            return executer.ExecuteWrite($"DELETE FROM Boards " +
                                         $"WHERE BoardId = {Id}; " +
                                         $"DELETE FROM UserJoinedBoards " +
                                         $"WHERE BoardId = {Id}; " +
                                         $"DELETE FROM Tasks " +
                                         $"WHERE BoardId = {Id}") ;
        }
        public bool JoinBoard(string email, int id)
        {
            log.Debug($"JoinBoard() for {email}, {id}");
            return executer.ExecuteWrite("INSERT into UserJoinedBoards (BoardId, Email) " +
                                        $"VALUES({id},'{email}')");
        }
        public bool LeaveBoard(string email, int id)
        {
            log.Debug($"LeaveBoard() for {email}, {id}");
            return executer.ExecuteWrite("DELETE FROM UserJoinedBoards " +
                                        $"WHERE BoardId = {id} and Email like '{email}'; " +
                                        $"UPDATE Tasks " +
                                        $"SET Assignee = 'unAssigned' " +
                                        $"WHERE BoardId = {id} and Assignee like '{email}'");
        }
        public bool ChangeOwner(string oldOwner,string newOwner, int id)
        {
            log.Debug($"ChangeOwner() for {oldOwner}, {newOwner}, {id}");
            return executer.ExecuteWrite("UPDATE Boards "+
                                        $"SET Owner = '{newOwner}' "+
                                        $"WHERE BoardId = {id}; " +
                                        $"DELETE FROM UserJoinedBoards " +
                                        $"WHERE BoardId = {id} and Email like '{newOwner}'; " +
                                        $"INSERT INTO UserJoinedBoards(BoardId, Email) " +
                                        $"VALUES({id},'{oldOwner}')");
        }
        public bool LimitColumn(int id, BoardColumnNames column, int limit)
        {
            log.Debug($"LimitColumn() for {id}, {column}, {limit}");
            return executer.ExecuteWrite("UPDATE Boards " +
                                        $"SET {column}Limit = {limit} " +
                                        $"WHERE BoardId = {id}");
        }
        public bool UpdateBoardIdCounter(int newValue)
        {
            log.Debug($"UpdateBoardIdCounter() for {newValue}");
            return executer.ExecuteWrite("UPDATE GlobalCounters " +
                                        $"SET BoardIDCounter = {newValue}");

        }
        public bool UpdateTaskIdCounter(int boardId, int newValue)
        {
            log.Debug($"AddBoard() for {boardId}, {newValue}");
            return executer.ExecuteWrite("UPDATE Boards " +
                                        $"SET TaskIDCounter = {newValue} " +
                                        $"WHERE BoardId = {boardId}");
        }

    }
}
