﻿using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.Utilities;
using IntroSE.Kanban.Backend.Exceptions;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    /// <summary>
	///This class implements TaskService 
	///<br/>
	///<code>Supported operations:</code>
	///<br/>
	/// <list type="bullet">UpdateTaskDueDate()</list>
	/// <list type="bullet">UpdateTaskTitle()</list>
    /// <list type="bullet">UpdateTaskDescription()</list>
	/// <br/><br/>
	/// ===================
	/// <br/>
	/// <c>Ⓒ Kfir Nissim</c>
	/// <br/>
	/// ===================
	/// </summary>
    /// 

    public class TaskService
    {
        private readonly TaskController taskController;

        public TaskService(TaskController TC)
        {
            taskController = TC;
        }


        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="emailRaw">Email of the user. Must be logged in</param>
        /// <param name="boardNameRaw">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>
		/// Json formatted as so:
		/// <code>
		///	{
		///		operationState: bool 
		///		returnValue: // (operationState == true) => empty string
		/// }			// (operationState == false) => error message		
		/// </code>
		/// </returns>
        public string UpdateTaskDueDate(string emailRaw, string boardNameRaw, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (ValidateArguments.ValidateNotNull(new object[] { emailRaw, boardNameRaw, columnOrdinal, taskId, dueDate }) == false)
            {
                Response<string> res = new(false, "UpdateTaskDueDate() failed: ArgumentNullException");
                return JsonController.ConvertToJson(res);
            }
            CIString email = new CIString(emailRaw);
            CIString boardName = new CIString(boardNameRaw);
            try
            {
                taskController.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                Response<string> res = new(true, "");
                return JsonController.ConvertToJson(res);
            }
            catch (NoSuchElementException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (ArgumentException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (IndexOutOfRangeException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (AccessViolationException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserDoesNotExistException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserNotLoggedInException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="emailRaw">Email of user. Must be logged in</param>
        /// <param name="boardNameRaw">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="titleRaw">New title for the task</param>
        /// <returns>
		/// Json formatted as so:
		/// <code>
		///	{
		///		operationState: bool 
		///		returnValue: // (operationState == true) => empty string
		/// }			// (operationState == false) => error message		
		/// </code>
		/// </returns>
        public string UpdateTaskTitle(string emailRaw, string boardNameRaw, int columnOrdinal, int taskId, string titleRaw)
        {
            if (ValidateArguments.ValidateNotNull(new object[] { emailRaw, boardNameRaw, columnOrdinal, taskId, titleRaw}) == false)
            {
                Response<string> res = new(false, "UpdateTaskTitle() failed: ArgumentNullException");
                return JsonController.ConvertToJson(res);
            }
            CIString email = new CIString(emailRaw);
            CIString boardName = new CIString(boardNameRaw);
            CIString title = new CIString(titleRaw);
            try
            {
                taskController.UpdateTaskTitle(email,boardName,columnOrdinal,taskId,title);
                Response<string> res = new(true, "");
                return JsonController.ConvertToJson(res);
            }
            catch (NoSuchElementException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (ArgumentException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (IndexOutOfRangeException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (AccessViolationException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserDoesNotExistException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserNotLoggedInException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="emailRaw">Email of user. Must be logged in</param>
        /// <param name="boardNameRaw">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="descriptionRaw">New description for the task</param>
        /// <returns>
		/// Json formatted as so:
		/// <code>
		///	{
		///		operationState: bool 
		///		returnValue: // (operationState == true) => empty string
		/// }			// (operationState == false) => error message		
		/// </code>
		/// </returns>
        public string UpdateTaskDescription(string emailRaw, string boardNameRaw, int columnOrdinal, int taskId, string descriptionRaw)
        {
            if (ValidateArguments.ValidateNotNull(new object[] { emailRaw, boardNameRaw, columnOrdinal, taskId, descriptionRaw }) == false)
            {
                Response<string> res = new(false, "UpdateTaskDescription() failed: ArgumentNullException");
                return JsonController.ConvertToJson(res);
            }
            CIString email = new CIString(emailRaw);
            CIString boardName = new CIString(boardNameRaw);
            CIString description = new CIString(descriptionRaw);
            try
            {
                taskController.UpdateTaskDescription(email,boardName,columnOrdinal,taskId,description);
                Response<string> res = new(true, "");
                return JsonController.ConvertToJson(res);
            }
            catch (NoSuchElementException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (ArgumentException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (IndexOutOfRangeException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (AccessViolationException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserDoesNotExistException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserNotLoggedInException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
        }


        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="emailRaw">Email of the user. Must be logged in</param>
        /// <param name="boardNameRaw">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified a task ID</param>        
        /// <param name="emailAssigneeRaw">Email of the asignee user</param>
        /// <returns>
        /// Json formatted as so:
        /// <code>
        ///	{
        ///		operationState: bool 
        ///		returnValue: // (operationState == true) => empty string
        /// }			// (operationState == false) => error message		
        /// </code>
        /// </returns>
        public string AssignTask(string emailRaw, string boardNameRaw, int columnOrdinal, int taskId, string emailAssigneeRaw)
        {
            if (ValidateArguments.ValidateNotNull(new object[] { emailRaw, boardNameRaw, columnOrdinal, taskId, emailAssigneeRaw }) == false)
            {
                Response<string> res = new(false, "UpdateTaskDescription() failed: ArgumentNullException");
                return JsonController.ConvertToJson(res);
            }
            CIString email = new CIString(emailRaw);
            CIString boardName = new CIString(boardNameRaw);
            CIString emailAssignee = new CIString(emailAssigneeRaw);
            try
            {
                taskController.AssignTask(email,boardName,columnOrdinal,taskId,emailAssignee);
                Response<string> res = new(true, "");
                return JsonController.ConvertToJson(res);
            }
            catch (NoSuchElementException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (AccessViolationException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserDoesNotExistException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (IndexOutOfRangeException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (ElementAlreadyExistsException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
            catch (UserNotLoggedInException ex)
            {
                Response<string> res = new(false, ex.Message);
                return JsonController.ConvertToJson(res);
            }
        }

    }
    
}
