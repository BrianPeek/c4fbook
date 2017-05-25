Light Sequencer v2.0
Written by Brian Peek (http://www.brianpeek.com/)
for the Animated Holiday Lights article
    at Coding4Fun (http://blogs.msdn.com/coding4fun/archive/2006/12/07/1230660.aspx)

Requirements
------------
    - 1 or more Phidget Interface Kit 0/0/4 devices
    - The hardware built using these devices in the Coding4Fun article.  You
      can find this article in the Coding4Fun Holiday section.
      (http://blogs.msdn.com/coding4fun/archive/2006/12/07/1230660.aspx)

Using the Software
------------------
Ensure that the Phidget devices you will be using are attached to the PC.  Start
by creating a new sequence from the File menu or by clicking the New Sequence
button.  In the dialog that appears, locate a music file to play back and enter
the length of time that the sequence should run.  If you are selecting a MIDI
file, the timing values will be set automatically.  Additionally, you may check
the "Hold notes?" box in order to have NoteOn commands held until a NoteOff
command is received.  Depending on the MIDI file, this will make the generated
sequence look better (or worse!).  Be sure to note which Phidget
devices are attached and which channels they map using the grid.  Click OK when
complete.

The screen will redraw and present the grid interface for the length of time
specified.  At this point, cells can be turned on and off by highlighting a cell
and right-clicking, or by pressing the "O" key to turn the cell On, and the "F"
key to turn the cell off.  Multiple cells can be selected and changed at once.

To use the recording interface, click the Record Sequence button or choose
Record Sequence from the Sequence menu.  Be sure to select the correct choice of
"Overwrite channel data" or "Append channel data."  As you record additional
channels, you will almost always want to append and not overwrite. 

Click the start button and a brief countdown will begin.  When the countdown
reaches 0, the music will begin.  A channel can be recorded by pressing the
appropriate keyboard key as follows:

1-0 --> Channels 1-10
Q-P --> Channels 11-20
A-; --> Channels 21-30
Z-/ --> Channels 31-40

When complete, press the Escape key, or click the Stop button.  When the Record
window is closed, the main grid will be updated with the sequence recorded.

Creating a sequence is certainly a time consuming task.  Each channel needs to
be recorded by hand.  While the rhythm interface allows one to record many
channels simultaneously, I think it would be impossible for anyone to type out
an entire sequence for all channels in one go.  In my opinion, it is easiest to
record one or two channels at a time and append the data as you go.  In the end,
you can use the grid interface to tweak the values and clean up any mistakes.

The sequence can be played back at any time. Simply press the Play button and
watch your holiday lights play back to the timing you created.  Press the Stop
button to end the current playback.

Sequences can be saved at any time by selecting Save from the "File" menu.

To test the channels by hand, select "Test Channels" from the "Tools" menu.
As with the recording screen, press the number keys associated with the channel
to turn on or off to test that channel.

To create a playlist of many sequences, select "New Playlist" from the "File"
menu.  Add as many .seq files as you wish to the listbox and save your playlist
file.  This can be reloaded later.  Use the "Play Control" options at the bottom
of the dialog box to start, stop, advance, or go back in the playlist.

Sample Sequences
----------------
You can find sample sequences at:

http://www.brianpeek.com/files/folders/sequences/default.aspx

Each sequences will consist of a sequence file (*.seq) and a music file.  Before
using, you will need to open the .seq file in a text editor and change the
"serialNumber" and "outputIndex" fields of each channel to the serial numbers
and output indexes of your own Phidget devices.  The serial numbers can be 
viewed in the "New Sequence" dialog box when plugged in.  The output indexes
are the channels on the board to which the wires are plugged in.

I will likely make this an automated process in the future if demand calls for
it.

History
-------
Changes since v1.21:
- New timing mode for increased accuracy
- Old sequences are still supported, but all new sequences will be created using
  the new timing methods
- Playlist support
- MIDI file format supported and auto-creation of sequences based on MIDI data

Changes since v1.2:
- Recompiled against latest Phidgets lib (11/7/2007)

Changes since v1.1:
- More bug fixes, especially to VB version
- Recording more than 10 channels allowed via keyboard...see below for the details

Changes since v1.0:
- Plenty of bug fixes as I created my own sequence
- A new "Test Channels" function so you can play with the channels at will
  outside of a recording
- Option to allow playback of already recorded channels while recording new
  channels
- Sample sequence included!  Read below on how to use it...

Contact Info
------------
Comments?  Bugs?  Feature requests?  Sequences?  Send 'em along!

Email: brian@brianpeek.com
Web:   http://www.brianpeek.com/
