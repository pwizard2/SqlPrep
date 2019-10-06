This is a simple tool to assist in conversion of  formatted Sql queries into line-broken strings for use in apps. 
That's it. I was previously doing this by hand nearly every day at work and this tool will make things easier. I built this with C#/MSSQL in mind but you can probably use it for other languages/databases as well with a little bit of adjustment. 

For best results, use this tool to "prepare" SQL that has already been reformatted by Poor SQL or a similar tool, since SqlPrep is designed to preserve the formatting/whitespace these formatting tools create. 

If you already have a formatted SQL string in your project's code and need to change it, you can also use SqlPrep to "strip" out the code elements (variable name, quotation marks, etc.) and convert the query string back into raw SQL so you can work on it in Sql Server Management Studio (or a similar IDE). Once you're done, SqlPrep can convert the Sql back into a ready-to-use string for you. 