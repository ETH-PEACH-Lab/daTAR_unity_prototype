<h1> datAR: An Embodied Approach for Teaching Data Literacy Through Everyday Objects </h1>
<p>
This file serves as documentation for the  tablet based AR application. Intended to teach data literacy to high school students through enganging with everyday objects and tangible analysis blocks.
</p>

<h3>Global Managers</h3>
These classes follow a singelton pattern and can be addressed in at any point in the application using the public static method called <em>Instance</em>

<h5>Collection Manager (local Database)</h5>
Using SimpleSQL plugin for sql integration into unity (see SimpleSQL folder for documention). The db always holds exactly one <em>Collection</em> table and potentaily a table for every row in the <em>Collection</em> table.

<h5>Node Manager</h5>
Has a regristration mechanism for returning nodes in the event when to matching nodes get connected through a screen interaction (drag and drop). Nodes refer to UI elemnts that can be connected by the user to trigger events or transfer data between different Blocks. There are seven types of nodes:
<ul>
<li>"CustomNode" (refering to knn Block output triangle)</li>
<li>"UserInput" (refering to knn "choose data point" panel)</li>
<li>"DataPoint" (refering to information overlay on a object)</li>
<li>"OpNode" (refering to basic analyis Blocks where, order by)</li>
<li>"ColumnNode" (refering to attribute names in AR data table)</li>
<li>"TableNode" (refering to AR table output triangle)</li>
<li>"VisNode" (refering to visualisation Block input triangle)</li>
</ul>
Possible connections between the node types are:
<ul>
<li>"CustomNode" <-> "VisNode"</li>
<li>"DataPoint" -> "UserInput"</li>
<li>"OpNode" <-> "ColumnNode"</li>
<li>"TableNode" <-> "VisNode"</li>
</ul>

<h5>Unit Manager</h5>

<h3>User Manager</h3>
Handels user login and stores user data in Player Prefabs. (Incomplete)

<h3>Image Tracking</h3>
<em>Reference Image Library</em>

<h3>Visualisation Blocks</h3>

<h3>Operation Blocks</h3>

<h3>Custom Blocks</h3>

<h3>Table Block</h3>

<h3>Object Marker</h3>

<h3>Bar code scanner</h3>

<h3>User</h3>

<h3>Collection overview UI</h3>

