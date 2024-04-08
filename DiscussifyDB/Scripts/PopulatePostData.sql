USE [BlogDatabase]
GO

SET IDENTITY_INSERT [dbo].[Post] ON
INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (1, 1, 'Coaching Advice', 
'<p><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018163933-1.png" 
style="height:340px; width:340px"/></p><p>&nbsp;</p><p><strong>PREPARING FOR BASKETBALL SEASON-COACHING ADVICE</strong></p>
<p>The most challenging seasons are the ones that leave you unprepared. According to research on preparing for big seasons and events,
one of the most effective tools are practice tests.&nbsp;</p><ul><li>What is the energy in the gym and the level of intensity your 
coaching inspires?</li><li>How quickly does your team follow through on your instructions?</li></ul>'
, 'Published', '20220118 10:34:09 AM', '20220220 11:30:10 AM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (2, 2, 'Deterministic Finite Automata', 
'<p><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018164707-2.jpeg" style="height:667px; width:1000px"/></p>
<h1><strong>Introduction to Deterministic Finite Automata (DFA)</strong></h1><h2>DFA explanation and example</h2><h2>What is a DFA?</h2>
<p>A DFA is a state machine consisting of&nbsp;<strong>states</strong>&nbsp;and&nbsp;<strong>transitions</strong>&nbsp;that can either 
accept or reject a finite string, which consists of a series of symbols, and compare it to a predefined language across a predetermined 
set of characters. We use&nbsp;<strong>circles&nbsp;</strong>to represent states, and<strong>&nbsp;directed arrows</strong>&nbsp;to 
represent transitions. Every state&nbsp;<em>must&nbsp;</em>have each symbol going outwards from the state, or else it will not be defined
as a DFA.</p>', 'Published', '20220401 01:25:11 PM', '20220405 01:45:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (3, 3, 'National Truck Driver Appreciation Week', 
'<h1><strong>Celebrating The Ones Who Keep Us Moving: 2022 National Truck Driver Appreciation Week.</strong></h1><p>Regardless of if you&rsquo;
re new to transportation or an industry veteran, it is easy to underestimate just how vital trucking is to our economy and daily lives.</p>
<p>As a refresher, here are a few key facts courtesy of the American Trucking Associations (ATA):</p><ul><li>There are 3.70 million class 
8 trucks on the road in the United States and 11.49 million commercial trailers</li><li>There are 892,078 for-hire carriers and 772,011 private 
carriers in the United States<ul><li>97.4% of these carriers have fewer than 20 trucks and 91.3% operate six trucks or less</li></ul></li></ul>'
, 'Unpublished', '20220512 12:28:09 AM', '20220520 01:00:10 AM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (4, 4, 'Effects of Eating on How You Sleep', 
'<p><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018165404-1.jpeg" style="height:428px; width:650px"/></p><h1><strong>
WHAT YOU EAT DETERMINES HOW YOU SLEEP, AND VICE-VERSA</strong></h1><p><em>Nutrition as the key to health is a well-known fact but did you know 
that it also has a direct link to the quality of your slumber?</em></p><p>Have you noticed how your eating habits change the day after a bad 
night&rsquo;s sleep? Chances are, you feel hungry often and reach for sweet, starchy, comfort food more than usual.</p><p>This is not just an 
emotional response but is actually backed by science. Lack of sleep causes leptin (which tells your body when you&rsquo;re full) to drop and 
ghrelin (a hormone that stimulates appetite) to rise.</p>', 'Unpublished', '20220824 05:01:55 PM', '20220829 06:30:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (5, 1, 'Work Out Routine', 
'<p><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018165404-1.jpeg" style="height:428px; width:650px"/></p><h1><strong>
10 REASONS TO WORK OUT IN THE MORNING</strong></h1><p><em>From better skin to deeper sleep, starting your day with a good exercise session has many 
positive effects on your health.</em></p><p>Turns out, it does make a difference when you start the day with a good sweat session.<br/>&nbsp;</p>
<h2>1. Enhance Fat-Burning</h2><p>A&nbsp;<a href="https://pubmed.ncbi.nlm.nih.gov/26844280/">study</a>&nbsp;in young, non-obese men found that those 
who exercised for an hour before breakfast experienced an increase in fat oxidisation compared to those who worked out after lunch or dinner. Other 
studies have also&nbsp;<a href="https://pubmed.ncbi.nlm.nih.gov/20837645/">shown</a>&nbsp;that exercising in a fasted state helps boost weight loss.
<br/>&nbsp;</p><h2>2. Unlock The Key To Consistency</h2><p>When you plan your workout for first thing in the morning, you&rsquo;re more likely to get 
it done as opposed to letting other commitments get in the way later in the day. That means lower chances of missing workouts or interrupting your progress.
<br/>&nbsp;</p><h2>3. Cultivate Better Habits</h2><p>Over time (some say 21 days, to be precise), that leads to the forming of morning exercise as a habit,
which will continue to serve you well in the long run. It will likely have a chain effect on other areas of your life as well, such as clean eating to 
complement your new exercise routine or just generally better discipline in seeing things through.</p><p>&nbsp;</p>', 'Unpublished', '20220824 04:10:55 PM', 
'20220829 06:45:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (6, 2, 'OCTA Strike', 
'<h1><strong>OCTA strike postponed as parties return to negotiating table</strong></h1><p>Talks between OCTA and Teamsters Local 952 initially failed Oct. 16
before both parties returned to contract talks at the Gov. Gavin Newsom&rsquo;s leadership team.</p><p>Orange County Transportation Authority (OCTA) bus riders
may have gone to bed thinking they would wake up to a county wide bus strike, but service is expected to operate as usual Monday as contract talks continue between
OCTA and Teamsters Local 952.</p><p>Negotiations during the evening of Oct. 16 had failed at one point with OCTA posting a notice on its website notifying riders of
a strike that was to begin at 12:01 Oct. 17. However, California Gov. Gavin Newsom&rsquo;s team stepped in and requested both parties return to the table to try to
find a resolution.</p>', 'Unpublished', '20220824 03:15:55 PM', '20220829 05:30:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (7, 3, 'Application Development and Emerging Technology', 
'<h1><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018170052-3.png" style="height:267px; width:501px"/></h1><p><strong>Application Development
and Emerging Technology</strong></p><p>This course focuses on the development of software application using the web and emerging technologies. Emphasis is on the 
requirements management, interface design, usability, deployment including ethical and legal considerations.</p><table cellpadding="0" cellspacing="0"><tbody><tr><td>Grid
</td><td>IDENTIFICATION: A structure made up of series of intersecting straight (vertical, horizontal and angular) or curved guide lines used to structure content.</td></tr>
<tr><th>&nbsp;</th><td>view</td><td>FILL IN THE BLANK: will be loaded by the controller passing the returned data from the model.</td></tr><tr><th>&nbsp;</th><td>data</td>
<td>An upload helper method that returns an array containing all of the data related to the file you uploaded.</td></tr><tr><th>&nbsp;</th><td>Slidedown()</td><td>IDENTIFICATION: 
This command is used to create a sliding down transition within a given time on a selected element</td></tr><tr><th>&nbsp;</th><td>set_value</td><td>Permits you to set the value
of an input form or textarea and used in form validation.</td></tr><tr><th>&nbsp;</th><td>FALSE</td><td>CI uses Model-Viewable-Controller architecture</td></tr><tr><th>&nbsp;
</th><td>TRUE</td><td>AJAX engine is an XMLHttpRequest object and jQuery makes it a lot more easier for us because of its AJAX development API.</td></tr><tr><th>&nbsp;</th><td>
$this-&gt;email-&gt;print_debugger()</td><td>returns a string containing any server messages, email header and the email message.</td></tr></tbody></table>', 'Published', 
'20220824 02:30:55 PM', '20220829 04:30:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (8, 4, 'Benefits of Playing Basketball', 
'<h1><br/><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018170326-4.png" style="height:340px; width:340px"/></h1><h1><strong>How Basketball Helps Our Wellbeing
</strong></h1><p>It improves our mental health</p><p>Did you know that playing basketball can improve not only your physique but also your mental health state? Physical activity helps to 
release endorphins, which are hormones that promote relaxation. They also help decrease stress and boost self-esteem, making you more likely to cope with everyday life more efficiently and 
improve your mood. Basketball also enables you to develop leadership skills, teamwork, and communication skills. It can even help you to deal with depression and anxiety.</p><p>Whether you 
are a professional athlete or an amateur, basketball is a great way to improve your mental health. A healthy lifestyle and a balanced social life can go a long way. Developing a solid 
self-awareness will help you perform better in the game and increase confidence, and a decent&nbsp;<a href="https://www.wowessays.com/" rel="noreferrer noopener" target="_blank">essay writing 
service</a>&nbsp;will help you find more time for training and reaping all of these benefits.</p>', 'Unpublished', '20220824 01:30:55 PM', '20220829 03:45:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (9, 1, 'Building Glutes', 
'<h1><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018170803-6.jpeg" style="height:428px; width:650px"/></h1><h1><strong>WHY YOU SHOULD BUILD YOUR GLUTES,
PLUS THE BEST EXERCISES FOR IT</strong></h1><p><em>Having strong glutes is important for a wide range of movements, and these 5 exercises are recommended by personal trainers.</em>
</p><h2><strong>Barbell Squats</strong></h2><p>In this move, you balance a weighted barbell on your upper breath while performing a squat. A compound exercise (that uses more than 
one muscle group at the same time); besides the glutes, it also works the hamstrings and lower back.<br/>&nbsp;</p><h2><strong>Single Leg Squats</strong></h2><p>Also known as pistol 
squat, this intermediate to advanced move is basically a squat that&rsquo;s performed while balancing on one leg, which further challenges your balance and stability.<br/>&nbsp;</p>
<h2><strong>Lunges</strong></h2><p>In this move, one leg stays anchored while the other moves forward, backward, side or crossover.</p>', 'Published', '20220824 04:00:55 PM', 
'20220829 05:40:10 PM');

INSERT [dbo].[Post] ([Id], [UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
VALUES (10, 2, 'Add MS SQL Server JSON Support To Entity Framework Core', 
'<h1><img src="https://ckeditor.com/apps/ckfinder/userfiles/files/image-20221018171107-7.jpeg" style="height:667px; width:1000px"/></h1><h2><strong>
<a href="https://khalidabuhakmeh.com/add-ms-sql-server-json-support-to-entity-framework-core" rel="bookmark">Add MS SQL Server JSON Support To Entity 
Framework Core</a></strong></h2><p>&nbsp;</p><p>While other database providers have made their place known within the .NET ecosystem, Microsoft&rsquo;
s SQL Server (MSSQL) and Azure flavor are still likely the most popular choice for .NET developers. The engine&rsquo;s popularity is because Microsoft&rsquo;
s SQL Server has long been a reliable choice with Microsoft support and a large tooling ecosystem, making it a safe option for developers and database administrators. 
As a result, there are few surprises when choosing MSSQL, but don&rsquo;t confuse that with being &ldquo;boring&rdquo;.</p><p>One of MSSQL&rsquo;s hidden gems is its 
ability to allow you to store and query JSON data in existing tables. Storing information as JSON can help reduce interaction complexity between your application and SQL, 
with storage objects typically a combination of identifier, sorting columns, and then a JSON column. JSON also lets you be more flexible about what you store, allowing you 
to store dynamic data that isn&rsquo;t easy with relational database schemas.</p><p>.NET Developers also utilize Entity Framework Core to access their database, allowing 
the library to generate SQL and map data into C# objects.</p><p>In this post, you&rsquo;ll see how to modify Entity Framework Core to support SQL Server&rsquo;s&nbsp;<code>
JSON_VALUE</code>&nbsp;and&nbsp;<code>JSON_QUERY</code>&nbsp;in your LINQ queries.</p>', 'Unpublished', '20220824 02:30:55 PM', '20220829 03:45:10 PM');
SET IDENTITY_INSERT [dbo].[Post] OFF