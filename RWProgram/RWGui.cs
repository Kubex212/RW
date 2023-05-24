using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RWProgram.Classes;
using Action = RWProgram.Classes.Action;
using State = RWProgram.Classes.State;
using RWProgram.Enums;
using LogicExpressionsParser;
namespace RWProgram
{
    public partial class RWGui : Form
    {
        public int ActorCounter { get; set; } = 0;

        public int ActionCounter { get; set; } = 0;

        public int FluentCounter { get; set; } = 0;

        public Query Query { get; set; } = null;

        public List<string> Statements = new List<string>()
        {
            "initially alpha",
            "alpha after A_1 by w_1, A_2 by w_2, ... ,A_n by w_n",
            "alpha typically after A_1 by w_1, A_2 by w_2, ... ,A_n by w_n",
            "observable alpha after A_1 by w_1, A_2 by w_2, ... ,A_n by w_n",
            "A by w causes alpha if π",
            "A by w releases f if π",
            "A by w typically causes alpha if π",
            "A by w typically release f if π",
            "impossible A by w if π",
            "always π",
            "noninertial fluent"
        };

        public List<string> Querends = new List<string>()
        {
            "always executable?",
            "ever executable?",
            "always accessible γ from π with P?",
            "ever accessible γ from π with P?",
            "typically accessible γ from π with P?",
            "always w involved with P?",
            "ever w involved with P?"
        };

        public List<string> Operators = new List<string>()
        {
            "∧",
            "∨",
            "→"
        };

        public Logic Logic { get; set; }
        
        public RWGui()
        {
            InitializeComponent();
            InitializeGui();
        }

        private void InitializeGui()
        {
            Logic = new Logic();
            StatementsComboBox.Items.Clear();
            StatementsComboBox.Items.AddRange(Statements.ToArray());
            QueriesComboBox.Items.Clear();
            QueriesComboBox.Items.AddRange(Querends.ToArray());
            Logic.Actors.Add(new Actor() { Name = "ɛ", Index = ActorCounter++ });
            Logic.Actors.Add(new Actor() { Name = "Anyone", Index = 1000 });
            ActorComboBox.Items.Clear();
            ActorComboBox.Items.AddRange(Logic.Actors.ToArray());
            ProgramActorComboBox.Items.Clear();
            ProgramActorComboBox.Items.AddRange(Logic.Actors.Where(n => n.Name != "Anyone" && n.Name != "ɛ").ToArray());
            Actor2ComboBox.Items.Clear();
            Actor2ComboBox.Items.AddRange(Logic.Actors.Where(n => n.Name != "Anyone" && n.Name != "ɛ").ToArray());
            Logic.Actions.Add(new Action() { Name = "Anything", Index = 1000 });
            ActionComboBox.Items.Clear();
            ActionComboBox.Items.AddRange(Logic.Actions.ToArray());
            ProgramActionComboBox.Items.Clear();
            ProgramActionComboBox.Items.AddRange(Logic.Actions.Where(n => n.Name != "Anything").ToArray());
        }

        private void ActorsTextBox_Changed(object sender, EventArgs e)
        {
            var actors_names = actorsTextBox.Text.Split(',',';','\n');
            actors_names = actors_names.Select(a => a.Trim()).Where(a => a != string.Empty).ToArray();

            var to_create = actors_names.Where(a => !Logic.Actors.Any(x => x.Name == a));
            Logic.Actors = Logic.Actors.Where(x => actors_names.Any(a => a == x.Name) || x.Name == "Anyone" || x.Name == "ɛ").ToList();
            foreach (var a in to_create)
                Logic.Actors.Add(new Actor() { Name = a, Index = ActorCounter++ });

            ActorComboBox.Items.Clear();
            ActorComboBox.Items.AddRange(Logic.Actors.ToArray());

            ProgramActorComboBox.Items.Clear();
            ProgramActorComboBox.Items.AddRange(Logic.Actors.Where(n => n.Name != "Anyone" && n.Name != "ɛ").ToArray());

            Actor2ComboBox.Items.Clear();
            Actor2ComboBox.Items.AddRange(Logic.Actors.Where(n => n.Name != "Anyone" && n.Name != "ɛ").ToArray());

            Logic.Program = Logic.Program.Where(x => Logic.Actors.Any(a => a.Index == x.actor.Index)).ToList();
            SetProgramText();
        }

        private void FluentsTextBox_Changed(object sender, EventArgs e)
        {
            var fluents_names = fluentsTextBox.Text.Split(',', ';', '\n');
            fluents_names = fluents_names.Select(f => f.Trim()).Where(f => f != string.Empty).ToArray();

            var to_create = fluents_names.Where(f => !Logic.Fluents.Any(x => x.Name == f));
            Logic.Fluents = Logic.Fluents.Where(x => fluents_names.Any(f => f == x.Name)).ToList();
            foreach (var f in to_create)
            {
                var fluent = new Fluent() { Name = f, Index = FluentCounter++};
                Logic.Fluents.Add(fluent);
                Logic.Fluents.Add(new NegatedFluent() { Name = f, Original = fluent, Index = fluent.Index });
            }
            FluentComboBox.Items.Clear();
            FluentComboBox.Items.AddRange(Logic.Fluents.ToArray());
        }

        private void ActionsTextBox_Changed(object sender, EventArgs e)
        {
            var actions_names = actionsTextBox.Text.Split(',', ';', '\n');
            actions_names = actions_names.Select(a => a.Trim()).Where(a => a != string.Empty).ToArray();

            var to_create = actions_names.Where(a => !Logic.Actions.Any(x => x.Name == a));
            Logic.Actions = Logic.Actions.Where(x => actions_names.Any(a => a == x.Name) || x.Name == "Anything").ToList();
            foreach (var f in to_create)
                Logic.Actions.Add(new Action() { Name = f, Index = ActionCounter++ });
            ActionComboBox.Items.Clear();
            ActionComboBox.Items.AddRange(Logic.Actions.ToArray());
            ProgramActionComboBox.Items.Clear();
            ProgramActionComboBox.Items.AddRange(Logic.Actions.Where(n => n.Name != "Anything").ToArray());

            Logic.Program = Logic.Program.Where(x => Logic.Actions.Any(a => a.Index == x.action.Index)).ToList();
            SetProgramText();
        }

        private void ClickAddStatementButton(object sender, EventArgs e)
        {
            HideButtons();
            var index = StatementsComboBox.SelectedIndex;
            StatementEnum statementEnum = (StatementEnum)index;

            string alphaString, piString;
            LogicalExpressionRoot alphaExpression, piExpression;
            State alphaState, piState;
            try
            {
                alphaString = AlphaTextBox.Text.Trim();
                if (string.IsNullOrEmpty(alphaString))
                    alphaState = new State();
                else
                {
                    alphaExpression = Parser.Parse(AlphaTextBox.Text);
                    alphaState = new State(alphaExpression, alphaString, Logic.Fluents);
                }

                piString = PiTextBox1.Text.Trim();
                if (string.IsNullOrEmpty(piString))
                    piState = new State();
                else
                {
                    piExpression = Parser.Parse(PiTextBox1.Text);
                    piState = new State(piExpression, piString, Logic.Fluents);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nastąpił błąd podczas parsowania podanej formuły", "Sprawdź formułę.",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Question);
                return;
            }

            switch (statementEnum)
            {
                case StatementEnum.InitiallyFluent:
                    if(!string.IsNullOrEmpty(alphaString))
                        Logic.Statements.Add(new InitiallyFluent (alphaState));
                    break;
                case StatementEnum.FluentAfterActionbyActor:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new FluentAfterActionbyActor
                    (   
                        alphaState,
                        new List<(Action action, Actor actor)> { ((Action)ActionComboBox.SelectedItem, (Actor)ActorComboBox.SelectedItem)}
                    ));
                    break;
                case StatementEnum.FluentTypicallyAfterActionbyActor:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new FluentTypicallyAfterActionbyActor
                    (
                        alphaState,
                        new List<(Action action, Actor actor)> { ((Action)ActionComboBox.SelectedItem, (Actor)ActorComboBox.SelectedItem)}
                    ));
                    break;
                case StatementEnum.ObservableFluentAfterActionByActor:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new ObservableFluentAfterActionByActor
                    (
                        alphaState,
                        new List<(Action action, Actor actor)> { ((Action)ActionComboBox.SelectedItem, (Actor)ActorComboBox.SelectedItem)}
                    ));
                    break;
                case StatementEnum.ActionByActorCausesAlphaIfFluents:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new ActionByActorCausesAlphaIfFluents
                    (
                        alphaState,
                        (Action)ActionComboBox.SelectedItem,
                        (Actor)ActorComboBox.SelectedItem,
                        piState
                    ));
                    break;
                case StatementEnum.ActionByActorReleasesFluent1IfFluents:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new ActionByActorReleasesFluent1IfFluents
                    (
                        (Fluent)FluentComboBox.SelectedItem,
                        (Action)ActionComboBox.SelectedItem,
                        (Actor)ActorComboBox.SelectedItem,
                        piState
                    ));
                    break;
                case StatementEnum.ActionByActorTypicallyCausesAlphaIfFluents:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new ActionByActorTypicallyCausesAlphaIfFluents
                    (
                        alphaState,
                        (Action)ActionComboBox.SelectedItem,
                        (Actor)ActorComboBox.SelectedItem,
                        piState
                    ));
                    break;
                case StatementEnum.ActionByActorTypicallyReleasesFluent1IfFluents:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new ActionByActorTypicallyReleasesFluent1IfFluents
                    (
                        (Fluent)FluentComboBox.SelectedItem,
                        (Action)ActionComboBox.SelectedItem,
                        (Actor)ActorComboBox.SelectedItem,
                        piState
                    ));
                    break;
                case StatementEnum.ImpossibleActionByActorIfFluents:
                    if (ActionComboBox.SelectedItem == null || ActorComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new ImpossibleActionByActorIfFluents
                    (
                        (Action)ActionComboBox.SelectedItem,
                        (Actor)ActorComboBox.SelectedItem,
                        piState
                    ));
                    break;
                case StatementEnum.AlwaysPi:
                    if (string.IsNullOrEmpty(piString))
                        return;
                    Logic.Statements.Add(new AlwaysPi
                    (
                        piState
                    ));
                    break;
                case StatementEnum.NoninertialFluent:
                    if (FluentComboBox.SelectedItem == null)
                        return;
                    Logic.Statements.Add(new NoninertialFluent
                    (
                        (Fluent)FluentComboBox.SelectedItem
                    ));
                    break;
                default:
                    break;
            }

            if (statementEnum == StatementEnum.FluentAfterActionbyActor || statementEnum == StatementEnum.FluentTypicallyAfterActionbyActor || statementEnum == StatementEnum.ObservableFluentAfterActionByActor)
                ShowButtons();

            SetStatementsText();
        }

        private void AddToProgramButton_Click(object sender, EventArgs e)
        {
            if (ProgramActionComboBox.SelectedItem != null && ProgramActorComboBox.SelectedItem != null)
            {
                Logic.Program.Add(((Action)ProgramActionComboBox.SelectedItem, (Actor)ProgramActorComboBox.SelectedItem));
                SetProgramText();
            }
        }

        private void SetStatementsText()
        {
            var str = string.Empty;
            foreach (var st in Logic.Statements)
                str = str + st.ToString() + "\n";

            StatementsTextBox.Text = str;
        }

        private void SetProgramText()
        {
            var str = string.Empty;
            foreach (var p in Logic.Program)
                str = str + p.Item1.ToString() + " by " + p.Item2.ToString() + "\n";
            ProgramTextBox.Text = str;
        }

        private void ResetStatementsButton_Click(object sender, EventArgs e)
        {
            Logic.Statements = new List<Statement>();
            SetStatementsText();
            ResetComboBoxes();
        }

        private void ResetComboBoxes()
        {
            ActionComboBox.ResetText();
            ActorComboBox.ResetText();
            FluentComboBox.ResetText();
            AlphaTextBox.Text = string.Empty;
            PiTextBox1.Text =  string.Empty;
        }

        private void StatementsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetComboBoxes();
            SetStatementsText();
        }

        private void ConfirmStatementButton_Click(object sender, EventArgs e)
        {
            HideButtons();
        }

        private void ResetProgramButton_Click(object sender, EventArgs e)
        {
            Logic.Program = new List<(Classes.Action, Actor)>();
            SetProgramText();
        }

        private void AddConditionButton_Click(object sender, EventArgs e)
        {
            var statementWIP = (StatementEnum)StatementsComboBox.SelectedIndex;
            switch (statementWIP)
            {
                case StatementEnum.FluentAfterActionbyActor:
                case StatementEnum.ObservableFluentAfterActionByActor:
                case StatementEnum.FluentTypicallyAfterActionbyActor:
                    {
                        var statement = Logic.Statements.Last();
                        var afterActionByActor = statement as StatementAfterActionByUser;
                        if (afterActionByActor != null)
                        {
                            afterActionByActor.ActionsByActors.Add(((Action)ActionComboBox.SelectedItem, (Actor)ActorComboBox.SelectedItem));
                        }
                        break;
                    }
                default:
                    break;
            }
            SetStatementsText();
        }

        private void ShowButtons()
        {
            ConfirmStatementButton.Visible = true;
            ConfirmStatementButton.Enabled = true;
            AddConditionButton.Visible = true;
            AddConditionButton.Enabled = true; 
        }

        private void HideButtons()
        {
            ConfirmStatementButton.Visible = false;
            ConfirmStatementButton.Enabled = false;
            AddConditionButton.Visible = false;
            AddConditionButton.Enabled = false;
            //LogicOperatorComboBox2.SelectedIndex = 0;
        }

        private void AskQueryButton_Click(object sender, EventArgs e)
        {
            if (Query != null && Logic?.Program != null)
            {
                var response = Logic.ExecuteQuery(Query);
                ResponseTextBox.Text = response.ToString();
            }
        }

        private void ResetQueryButton_Click(object sender, EventArgs e)
        {
            Query = null;
            //ResetButtons();
            QueriesComboBox.SelectedIndex = -1;
            ResponseTextBox.Text = string.Empty;
            ResetComboBoxes2();
        }

        private void AddQueryButton_Click(object sender, EventArgs e)
        {
            var queryIndex = QueriesComboBox.SelectedIndex;
            var queryEnum = (QueriesEnum)queryIndex;

            string gammaString, piString;
            LogicalExpressionRoot gammaExpression, piExpression;
            State gammaState, piState;
            try
            {
                gammaString = GammaTextBox.Text.Trim();
                if(string.IsNullOrEmpty(gammaString))
                    gammaState = new State();
                else
                {
                    gammaExpression = Parser.Parse(GammaTextBox.Text);
                    gammaState = new State(gammaExpression, gammaString, Logic.Fluents);
                }

                piString = PiTextBox2.Text.Trim();
                if (string.IsNullOrEmpty(piString))
                    piState = new State();
                else
                {
                    piExpression = Parser.Parse(PiTextBox1.Text);
                    piState = new State(piExpression, piString, Logic.Fluents);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nastąpił błąd podczas parsowania podanej formuły", "Sprawdź formułę.",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Question);
                return;
            }

            switch (queryEnum)
            {
                case QueriesEnum.AlwaysExecutable:
                    Query = new AlwaysExecutable();
                    break;
                case QueriesEnum.EverExecutable:
                    Query = new EverExecutable();
                    break;
                case QueriesEnum.AlwaysAccesibleYFromPi:
                    if (!string.IsNullOrEmpty(gammaString))
                    {
                        var QueryAlways = new AlwaysAccesibleYFromPi(gammaState, piState);
                        Query = QueryAlways;
                    }
                    break;
                case QueriesEnum.EverAccesibleYFromPi:
                    if (!string.IsNullOrEmpty(gammaString))
                    {
                        var QueryAlways = new EverAccesibleYFromPi(gammaState, piState);
                        Query = QueryAlways;
                    }
                    break;
                case QueriesEnum.TypicallyAccesibleYFromPi:
                    if (!string.IsNullOrEmpty(gammaString))
                    {
                        var QueryAlways = new TypicallyAccesibleYFromPi(gammaState, piState);
                        Query = QueryAlways;
                    }
                    break;
                case QueriesEnum.AlwaysWInvolved:
                    if (Actor2ComboBox.SelectedItem != null)
                        Query = new AlwaysWInvolved() { W = (Actor)Actor2ComboBox.SelectedItem };
                    break;
                case QueriesEnum.EverWInvolved:
                    if (Actor2ComboBox.SelectedItem != null)
                        Query = new EverWInvolved() { W = (Actor)Actor2ComboBox.SelectedItem };
                    break;
                default:
                    break;
            }

            SetQueryTextBox();
        }

        //private void ResetButtons()
        //{
        //    AddInitialStateButton.Visible = false;
        //    AddCondition2Button.Visible = false;
        //    AddInitialStateButton.Enabled = false;
        //    AddCondition2Button.Enabled = false;
        //}

        private void QueriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query = null;
            SetQueryTextBox();
            ResetComboBoxes2();
            //var queryIndex = QueriesComboBox.SelectedIndex;
            //var queryEnum = (QueriesEnum)queryIndex;
            //SetQueryTextBox();
            //ResetButtons();
            //switch (queryEnum)
            //{
            //    case QueriesEnum.AlwaysAccesibleYFromPi:
            //    case QueriesEnum.EverAccesibleYFromPi:
            //        AddInitialStateButton.Visible = true;
            //        AddCondition2Button.Visible = true;
            //        AddInitialStateButton.Enabled = true;
            //        AddCondition2Button.Enabled = true;
            //        break;
            //    default:
            //        break;
            //}
        }

        //private void AddInitialStateButton_Click(object sender, EventArgs e)
        //{
        //    var queryIndex = QueriesComboBox.SelectedIndex;
        //    var queryEnum = (QueriesEnum)queryIndex;
        //    switch (queryEnum)
        //    {

        //        case QueriesEnum.AlwaysAccesibleYFromPi:
        //        case QueriesEnum.EverAccesibleYFromPi:
        //            {
        //                var query = Query as QueryWithGammaAndPi;
        //                if (query != null)
        //                {
        //                    query.Pi.Add((Fluent)Pi2ComboBox.SelectedItem);
        //                }
        //                break;
        //            }
        //        default:
        //            break;
        //    }

        //    SetQueryTextBox();
        //}

        //private void AddCondition2Button_Click(object sender, EventArgs e)
        //{
        //    var queryIndex = QueriesComboBox.SelectedIndex;
        //    var queryEnum = (QueriesEnum)queryIndex;
        //    switch (queryEnum)
        //    {

        //        case QueriesEnum.AlwaysAccesibleYFromPi:
        //        case QueriesEnum.EverAccesibleYFromPi:
        //            {
        //                var query = Query as QueryWithGammaAndPi;
        //                if (query != null)
        //                {
        //                    if (GammaComboBox.SelectedItem != null)
        //                    {
        //                        query.Gamma.Add((Fluent)GammaComboBox.SelectedItem);
        //                    }

        //                }
        //                break;
        //            }
        //        default:
        //            break;
        //    }

        //    SetQueryTextBox();
        //}


        private void SetTest1Domain(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test1);
        }

        private void SetTest2Domain(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test2);
        }

        private void SetTest3Domain(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test3);
        }

        private void SetTest4Domain(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test4);
        }

        private void SetTest5Domain(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test5);
        }

        private void ResetComboBoxes2()
        {
            Actor2ComboBox.ResetText();
            PiTextBox2.Text = string.Empty;
            GammaTextBox.Text = string.Empty;
        }

        private void SetQueryTextBox()
        {
            QueryTextBox.Text = Query != null ? Query.ToString() + $" with program: {string.Join(",",Logic.Program.Select(x => $" {x.action.Name} by {x.actor.Name}"))}?" : string.Empty;
        }

        private void DeleteLastStatementButton_Click(object sender, EventArgs e)
        {
            if (Logic.Statements.Count > 0)
                Logic.Statements.RemoveAt(Logic.Statements.Count - 1);
            SetStatementsText();
        }

        private void DeleteLastProgramButton_Click(object sender, EventArgs e)
        {
            if (Logic.Program.Count > 0)
                Logic.Program.RemoveAt(Logic.Program.Count - 1);
            SetProgramText( );
        }

        private void SetHardcodedLogicInFrontEnd(Logic logic)
        {
            actorsTextBox.Text = string.Join(", ", logic.Actors.Where(a => a.Name != "Anyone" && a.Name != "ɛ").Select(x => x.ToString()));
            ActorsTextBox_Changed(null, null);
            fluentsTextBox.Text = string.Join(", ", logic.Fluents.Where(f => !(f is NegatedFluent)).Select(x => x.ToString()));
            FluentsTextBox_Changed(null, null);
            actionsTextBox.Text = string.Join(", ", logic.Actions.Where(a => a.Name != "Anything").Select(x => x.ToString()));
            ActionsTextBox_Changed(null, null);
            Logic = logic;
            SetStatementsText();
        }

        private void SetHardcodedProgramInFrontEnd(List<(Action action, Actor actor)> Program)
        {
            Logic.Program = Program;
            SetProgramText();
        }

        private void SetHardcodedQueryInFrontEnd(Query query)
        {
            Query = query;
            SetQueryTextBox();
        }

        private void przykładKwerenda1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test1);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test1a);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test1a);
        }

        private void przykładKwerenda2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test1);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test1b);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test1b);
        }

        private void przykładKwerenda3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test1);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test1c);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test1c);
        }

        private void przykładKwerenda1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test2);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test2a);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test2a);
        }

        private void przykładKwerenda2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test2);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test2b);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test2b);
        }

        private void przykładKwerenda3ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test2);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test2c);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test2c);
        }

        private void przykładKwerenda1ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test3);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test3a);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test3a);
        }

        private void przykładKwerenda2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test3);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test3b);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test3b);
        }

        private void przykładKwerenda3ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test3);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test3c);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test3c);
        }

        private void przykładKwerenda1ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test4);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test4a);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test4a);
        }

        private void przykładKwerenda2ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test4);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test4b);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test4b);
        }

        private void przykładKwerenda3ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test4);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test4c);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test4c);
        }

        private void przykładKwerenda1ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test5);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test5a);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test5a);
        }

        private void przykładKwerenda2ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test5);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test5b);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test5b);
        }

        private void przykładKwerenda3ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SetHardcodedLogicInFrontEnd(Tests_Logic.Test5);
            SetHardcodedProgramInFrontEnd(Tests_Programs.Test5c);
            SetHardcodedQueryInFrontEnd(Tests_Queries.Test5c);
        }

        private void ResetAllButton_Click(object sender, EventArgs e)
        {
            //TODO: refactor?
            ActorCounter = 0;
            ActionCounter = 0;
            FluentCounter = 0;
            actorsTextBox.Text = String.Empty;
            fluentsTextBox.Text = String.Empty;
            actionsTextBox.Text = String.Empty;
            InitializeGui();
            SetStatementsText();
            ResetComboBoxes();
            SetProgramText();
            ResetComboBoxes2();
            FluentComboBox.Items.Clear();
            Query = null;
            QueriesComboBox.ResetText();
            ResponseTextBox.Text = string.Empty;
            StatementsComboBox.ResetText();
            ProgramActionComboBox.ResetText();
            ProgramActorComboBox.ResetText();
            SetQueryTextBox();
        }
    }
}
