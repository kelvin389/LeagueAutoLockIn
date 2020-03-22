﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace League
{
    public partial class RunesForm : Form
    {
        List<RuneTree> runeTrees = new List<RuneTree>();
        List<RadioButton> radioButtons = new List<RadioButton>();
        List<GroupBox> primaryGroupBoxes = new List<GroupBox>();
        List<GroupBox> secondaryGroupBoxes = new List<GroupBox>();
        RuneTree primaryTree;
        RuneTree secondaryTree;
        const int PRIMARY_START_X = 10;
        const int PRIMARY_X_OFFSET = 130;
        const int PRIMARY_GROUPBOX_START_Y = 100;
        const int PRIMARY_GROUPBOX_START_X = 50;
        const int PRIMARY_GROUPBOX_OFFSET = 50;
        const int SECONDARY_START_X = 10;
        const int SECONDARY_X_OFFSET = 130;
        const int SECONDARY_GROUPBOX_START_Y = 50;
        const int SECONDARY_GROUPBOX_START_X = 650;
        const int SECONDARY_GROUPBOX_OFFSET = 50;

        public RunesForm()
        {
            InitializeComponent();
        }

        public void ParseRunesJson()
        {
            //string jsondir = Directory.GetCurrentDirectory() + "/data/champion.json";
            string jsondir = "C:/Users/kelvi/Documents/Visual Studio 2019/Projects/League/League/resources/runesReforged.json";
            string json = System.IO.File.ReadAllText(jsondir);

            var trees = JArray.Parse(json);

            var curTree = trees.First;

            // iterate through each rune tree (prec, dom, sorc, res, insp)
            for (int k = 0; k < trees.Count(); k++)
            {
                string treeName = curTree["name"].ToString();
                int treeId = Convert.ToInt32(curTree["id"].ToString());
                string treeIconPath = curTree["icon"].ToString();

                var slots = curTree["slots"];
                var curSlot = slots.First;

                List<RuneRow> rows = new List<RuneRow>();

                // iterate through each row of runes (keystones, secondaries 1, secondaries 2, secondaries 3)
                for (int j = 0; j < slots.Count(); j++)
                {
                    List<Rune> runes = new List<Rune>();
                    var curRune = curSlot["runes"].First;

                    // iterate through each rune in a row (pta, lt, fleet, conq)
                    for (int i = 0; i < curSlot["runes"].Count(); i++)
                    {
                        int runeId = Convert.ToInt32(curRune["id"]);
                        string runeIconPath = curRune["icon"].ToString();
                        string runeName = curRune["name"].ToString();
                        string runeKey = curRune["key"].ToString();

                        Rune newRune = new Rune(runeId, runeIconPath, runeName, runeKey);
                        runes.Add(newRune);
                        curRune = curRune.Next; // go to next rune in row
                    }
                    RuneRow row = new RuneRow(runes);
                    rows.Add(row);
                    curSlot = curSlot.Next; // go to next row
                }
                RuneTree set = new RuneTree(rows, treeId, treeName, treeIconPath);
                runeTrees.Add(set);
                curTree = curTree.Next; // go to next tree
            }
        }

        private void UpdatePrimary()
        {
            // clean up previous group boxes and radiobuttons
            for (int i = 0; i < primaryGroupBoxes.Count; i++)
            {
                Controls.Remove(primaryGroupBoxes[i]);
            }

            // iterate through each row of runes
            for (int i = 0; i < primaryTree.runeRows.Count; i++)
            {
                List<Rune> thisRowRunes = primaryTree.runeRows[i].runes;
                List<RadioButton> thisRowRadioButtons = new List<RadioButton>();
                int curX = PRIMARY_START_X;
                if (thisRowRunes.Count == 3) curX += (int)(PRIMARY_X_OFFSET / 2.0f);

                // iterate through each rune in this row
                for (int j = 0; j < thisRowRunes.Count; j++)
                {
                    // create new radio button for each rune
                    RadioButton button = new RadioButton
                    {
                        Location = new System.Drawing.Point(curX, 20),
                        AutoSize = true,
                        Tag = thisRowRunes[j].id,
                        Text = thisRowRunes[j].name
                    };
                    radioButtons.Add(button);
                    thisRowRadioButtons.Add(button);
                    curX += PRIMARY_X_OFFSET;
                }

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle()
                {
                    X = PRIMARY_GROUPBOX_START_X,
                    Y = PRIMARY_GROUPBOX_START_Y + (PRIMARY_GROUPBOX_OFFSET * i), // move down as number of groupboxes already created increases
                };
                GroupBox box = new GroupBox()
                {
                    Bounds = rect,
                    AutoSize = true,
                    MinimumSize = new System.Drawing.Size(520, 0) // force all sizes of boxes to be 500px wide
                };
                for (int k = 0; k < thisRowRadioButtons.Count; k++)
                {
                    //this.Controls.Add(thisRowRadioButtons[k]);
                    box.Controls.Add(thisRowRadioButtons[k]);
                }
                this.Controls.Add(box);
                primaryGroupBoxes.Add(box);
            }
        }

        private void UpdateSecondary()
        {
            // clean up previous group boxes and radiobuttons
            for (int i = 0; i < secondaryGroupBoxes.Count; i++)
            {
                Controls.Remove(secondaryGroupBoxes[i]);
            }

            // iterate through each row of runes
            // start at 1 to skip keystones
            for (int i = 1; i < secondaryTree.runeRows.Count; i++)
            {
                List<Rune> thisRowRunes = secondaryTree.runeRows[i].runes;
                List<RadioButton> thisRowRadioButtons = new List<RadioButton>();
                int curX = SECONDARY_START_X;
                if (thisRowRunes.Count == 3) curX += (int)(SECONDARY_X_OFFSET / 2.0f);

                // iterate through each rune in this row
                for (int j = 0; j < thisRowRunes.Count; j++)
                {
                    // create new radio button for each rune
                    RadioButton button = new RadioButton
                    {
                        Location = new System.Drawing.Point(curX, 20),
                        AutoSize = true,
                        Tag = thisRowRunes[j].id,
                        Text = thisRowRunes[j].name
                    };
                    radioButtons.Add(button);
                    thisRowRadioButtons.Add(button);
                    curX += SECONDARY_X_OFFSET;
                }

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle()
                {
                    X = SECONDARY_GROUPBOX_START_X,
                    Y = SECONDARY_GROUPBOX_START_Y + (SECONDARY_GROUPBOX_OFFSET * i), // move down as number of groupboxes already created increases
                };
                GroupBox box = new GroupBox()
                {
                    Bounds = rect,
                    AutoSize = true,
                    MinimumSize = new System.Drawing.Size(520, 0) // force all sizes of boxes to be 500px wide
                };
                for (int k = 0; k < thisRowRadioButtons.Count; k++)
                {
                    //this.Controls.Add(thisRowRadioButtons[k]);
                    box.Controls.Add(thisRowRadioButtons[k]);
                }
                this.Controls.Add(box);
                secondaryGroupBoxes.Add(box);
            }
        }

        private void RunesForm_Load(object sender, EventArgs e)
        {

        }

        private void primary0_CheckedChanged(object sender, EventArgs e)
        {
            if (primary0.Checked)
            {
                primaryTree = runeTrees[0];
                UpdatePrimary();
            }
        }

        private void primary1_CheckedChanged(object sender, EventArgs e)
        {
            if (primary1.Checked)
            {
                primaryTree = runeTrees[1];
                UpdatePrimary();
            }
        }

        private void primary2_CheckedChanged(object sender, EventArgs e)
        {
            if (primary2.Checked)
            {
                primaryTree = runeTrees[2];
                UpdatePrimary();
            }
        }

        private void primary3_CheckedChanged(object sender, EventArgs e)
        {
            if (primary3.Checked)
            {
                primaryTree = runeTrees[3];
                UpdatePrimary();
            }
        }

        private void primary4_CheckedChanged(object sender, EventArgs e)
        {
            if (primary4.Checked)
            {
                primaryTree = runeTrees[4];
                UpdatePrimary();
            }
        }

        private void secondary0_CheckedChanged(object sender, EventArgs e)
        {
            if (secondary0.Checked)
            {
                secondaryTree = runeTrees[0];
                UpdateSecondary();
            }
        }

        private void secondary1_CheckedChanged(object sender, EventArgs e)
        {
            if (secondary1.Checked)
            {
                secondaryTree = runeTrees[1];
                UpdateSecondary();
            }
        }

        private void secondary2_CheckedChanged(object sender, EventArgs e)
        {
            if (secondary2.Checked)
            {
                secondaryTree = runeTrees[2];
                UpdateSecondary();
            }
        }

        private void secondary3_CheckedChanged(object sender, EventArgs e)
        {
            if (secondary3.Checked)
            {
                secondaryTree = runeTrees[3];
                UpdateSecondary();
            }
        }

        private void secondary4_CheckedChanged(object sender, EventArgs e)
        {
            if (secondary4.Checked)
            {
                secondaryTree = runeTrees[4];
                UpdateSecondary();
            }
        }
    }

    
}

/*
 * Keystones and minor runes radio buttons should all
 * be generated on runtime to futureproof
 */

/* https://stackoverflow.com/questions/18547326/how-do-i-get-which-radio-button-is-checked-from-a-groupbox
 * find all checked buttons
 * 
 * var buttons = this.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
 */
