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
Handels data points that should be highlighted in a data visualization, that is data points that are attached to an physical object.

<h3>User Manager</h3>
Handels user login and stores user data in Player Prefabs. (Incomplete)

<h3>Image Tracking</h3>
<em>Reference Image Library</em>- local db of AR markers the application is tracking, provided by AR foundations plugin. 

Names for each image matter and should be updated in the <em>ImageTracking.cs</em> script. This script handles the initialization and update of AR objects for the corresponding AR markers. (Remark: the primitive caching behaviour for AR objects has different bahaviour on iOS(strange) vs Android(better))

When adding a new image to the library make sure to have a good resolution (min 600px*600px) and specify physical size of the printed image.

<h3>Visualisation Blocks</h3>
There are two types of Visualisation Blocks each associated with one Manager script, that is <em>VisBlockManager.cs</em> and <em>CustomVisManager.cs</em>.
The Manager script handels all the UI elements of the Block also initialising and updating the data visualisation refered to as <em>charts</em>.

<h5>IChart</h5>
Interface implemented by all chart Manager scripts, that is:
<ul>
<li>ScatterPlotManager (inizilased by VisBlockManager)</li>
<li>BarChartManager (inizialised by VisBlockManager)</li>
<li>PieChartManger (inizialised by CustomVisManager)</li>
<li>CardChartManger (initialised by ObjectManager, it is the AR text overlay attached to physical object)</li>
</ul>
Each chart has its own prefab with a template of the UI element representing a single data point with dummy data. When populating the chart the template gets cloned based on the real data.

<h3>Operation Blocks</h3>
<h5>IOpManger</h5>
Interface implented by all the data table operation blocks Manger scripts, that is <em>OrderbyManager.cs</em> and <em>WhereManager.cs</em>. Stores the AR table column node the block is connected to in order to send sql quers operations to the AR data table block.

<h3>Custom Blocks</h3>
Existing custom blocks are custom data visualisation pie chart (handled by <em>CustomVisManager.cs</em>) and kNN analysis block (handled by <em>CustomBlockManager.cs</em>). Each of the Manger scripts implements the <em>IBlockManager</em> interface. Every custom block stores a potentialy connected data point node (i.e. when connecting a physical object to a pie chart for visualisation).

The <em>CustomBlockManger.cs</em> script follows this sequence of events, in order for enabling the creation of custom analysis blocks:
<ol>
<li>initConstructorView()</li>
<li>executeConstructor(Dictionary<string, string> selectedParams)</li>
<li>initMethodView()</li>
<li>executeMethod(Dictionary<string, string> selectedParams)</li>
</ol>

<h3>Table Block</h3>

<h3>Object Marker</h3>

<h3>Bar Code Scanner</h3>

<h3>Import csv Files</h3>
On Android devices straight forward just put csv file anywhere in the device, than you can access the file through the file browser in the application.

On iOS device because of sandbox approach not possible to access files on the the device through the file browser in the application. Extra script needed to load csv file from the StreamingAssets folder to a folder created during the build of the application on the deployed device.

