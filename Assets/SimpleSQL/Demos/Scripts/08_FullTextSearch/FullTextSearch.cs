namespace SimpleSQL.Demos
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class FullTextSearch : MonoBehaviour
	{

		public SimpleSQL.SimpleSQLManager dbManager;

		public Text outputText;

		void Start()
		{
			dbManager.Execute("DROP TABLE IF EXISTS posts");

			dbManager.Execute("CREATE VIRTUAL TABLE posts"
													+ " USING FTS5 ("
													+ "title"
													+ ",body"
													+ ")"
													);

			dbManager.Execute("INSERT INTO posts "
												+ "("
												+ "title"
												+ ",body"
												+ ")"
												+ " VALUES ("
												+ "'Learn SQlite FTS5'"
												+ ",'This tutorial teaches you how to perform full-text search in SQLite using FTS5'"
												+ ")"
												+ ",("
												+ "'Advanced SQlite Full-text Search'"
												+ ",'Show you some advanced techniques in SQLite full-text searching'"
												+ ")"
												+ ",("
												+ "'SQLite Tutorial'"
												+ ",'Help you learn SQLite quickly and effectively'"
												+ ")"
												);

			var matchResults = dbManager.Query<Post>("SELECT * FROM posts WHERE posts MATCH 'learn NOT text'");

			outputText.text = "Posts matching 'learn NOT text'\n\n";
			foreach (var result in matchResults)
			{
				outputText.text += "<color=#1abc9c>Title</color>: '" + result.title + "' " +
									"<color=#1abc9c>Body</color>: '" + result.body + "'\n";
			}
		}
	}
}