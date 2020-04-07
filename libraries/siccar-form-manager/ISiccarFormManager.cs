using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Siccar.FormManager
{
    public interface ISiccarFormManager
    {
        /// <summary>
        /// Builds the values passed to it into the correct format.
        /// Please Note
        ///     Any field that starts with 
        ///     xxx
        ///     previousStepId
        ///     __RequestVerificationToken
        ///     Will not be added as Siccar will generate a 400 Bad Request
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="stepComment"></param>
        /// <returns></returns>
        dynamic BuildFormSubmissionModel(IFormCollection coll, string stepComment = "");

        /// <summary>
        /// Returns the field value form the object that has just been built
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        string ReturnFieldFromSubmission(IFormCollection coll, string key);

        /// <summary>
        /// If your trying to submit a model and you dont know how to make a FormCollection
        /// Suggestion is to create a ToDictionary method in your model and then call this method.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        dynamic BuildFormSubmissionModel(Dictionary<string, StringValues> elements, string stepComment = "");


    }
}