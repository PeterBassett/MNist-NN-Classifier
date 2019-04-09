# Neural Network using back propagation and gradient descent training.

A fun little project to build a classifier for hand written numbers.

!["User Interface?"](/Screenshot.png?raw=true)

Using the training data available at http://yann.lecun.com/exdb/mnist/ this little
app can train a couple of different network types to identify a digit drawn by the user.

Start the app and let it train for a while.
Draw a single digit number in the left hand black box. The app will redraw it centered on the middle box.
That will be turned into a thumbnail 28x28px and passed off the the currently training Neural Network.

The simple network is a hardcoded three layer 784->64->10 node network. The MultiLayerNeuralNetwork class
does the same job but can have 3 or more layers of any number of nodes. The app enforces the sizes of the
input (784 nodes) and output (10 nodes) layers because that is what we need for this classifier.

Dont look too closely at the WinForms code, it's pretty thrown together just to see some output. ;-)

The math behind back propagation is somewhat in depth, I'll more of an explanation here in time but here are some useful resources:

https://www.youtube.com/playlist?list=PLZHQObOWTQDNU6R1_67000Dx_ZCJB-3pi

https://www.youtube.com/playlist?list=PLRqwX-V7Uu6Y7MdSCaIfsxc561QI0U0Tb
