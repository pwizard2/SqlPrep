This is a simple tool to assist in conversion of  formatted Sql queries into line-broken strings for use in apps. 
That's it. I was previously doing this by hand nearly every day at work and this tool will make things easier. I built this with C#/MSSQL in mind but you can probably use it for other languages/databases as well with a little bit of adjustment. 

For best results, use this tool to process SQL that has already been reformatted by Poor SQL or a similar tool, since SqlPrep is designed to preserve the formatting/whitespace these formatting tools create.