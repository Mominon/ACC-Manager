﻿using ACCSetupApp.SetupParser.SetupRanges;
using ACCSetupApp.SetupParser.Cars.GT3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ACCSetupApp.SetupParser.SetupConverter;
using MaterialDesignThemes.Wpf;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using static SetupParser.SetupJson;

namespace ACCSetupApp.Controls
{
    /// <summary>
    /// Interaction logic for SetupEditor.xaml
    /// </summary>
    public partial class SetupEditor : UserControl
    {
        private static SetupEditor _instance;
        public static SetupEditor Instance
        {
            get
            {
                return _instance;
            }
        }

        private ISetupChanger SetupChanger { get; set; }
        private Root Setup { get; set; }

        public SetupEditor()
        {
            InitializeComponent();
            _instance = this;
        }

        public void Open(string file)
        {
            this.Setup = GetSetup(new FileInfo(file));

            Instance.transitionEditPanel.Visibility = Visibility.Visible;
            SetupChanger = new Porsche911IIGT3R();
            CreateFields();
        }

        public void Save()
        {

        }

        public void Close()
        {

        }

        private void CreateFields()
        {
            FieldStackPanel.Children.Clear();

            // Tyre Setup
            int tyreLabelWidth = 110;
            FieldStackPanel.Children.Add(GetTyrePressureStacker(tyreLabelWidth));
            FieldStackPanel.Children.Add(GetToeStacker(tyreLabelWidth));
            FieldStackPanel.Children.Add(GetCamberStacker(tyreLabelWidth));
            FieldStackPanel.Children.Add(GetCasterStacker(tyreLabelWidth));


            // Mechanical Setup
            int mechLabelWidth = 110;
            FieldStackPanel.Children.Add(GetWheelRatesStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetBumpstopRateStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetBumpstopRangeStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetAntiRollBarStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetDiffPreloadStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetBrakePowerStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetBrakeBiasStacker(mechLabelWidth));
            FieldStackPanel.Children.Add(GetSteeringRatioStacker(mechLabelWidth));


            // Aero Setup

        }


        #region MechanicalSetupChanger
        private Grid GetSteeringRatioStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Steering Ratio", labelWidth);

            Grid settings = GetGrid(1, 90);
            Grid.SetColumn(settings, 1);


            StackPanel stackerBrakeBias = new StackPanel { Orientation = Orientation.Horizontal };
            ComboBox comboBrakeBias = new ComboBox() { Width = 88, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboBrakeBias.ItemsSource = GetDoubleRangeCollection(SetupChanger.MechanicalSetupChanger.SteeringRatio);
            comboBrakeBias.SelectedIndex = Setup.basicSetup.alignment.steerRatio;
            comboBrakeBias.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.steerRatio = comboBrakeBias.SelectedIndex; };
            stackerBrakeBias.Children.Add(comboBrakeBias);
            Grid.SetColumn(stackerBrakeBias, 0);


            settings.Children.Add(stackerBrakeBias);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetBrakeBiasStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Brake Bias", labelWidth);

            Grid settings = GetGrid(1, 90);
            Grid.SetColumn(settings, 1);


            StackPanel stackerBrakeBias = new StackPanel { Orientation = Orientation.Horizontal };
            ComboBox comboBrakeBias = new ComboBox() { Width = 88, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboBrakeBias.ItemsSource = GetDoubleRangeCollection(SetupChanger.MechanicalSetupChanger.BrakeBias);
            comboBrakeBias.SelectedIndex = Setup.advancedSetup.mechanicalBalance.brakeBias;
            comboBrakeBias.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.brakeBias = comboBrakeBias.SelectedIndex; };
            stackerBrakeBias.Children.Add(comboBrakeBias);
            Grid.SetColumn(stackerBrakeBias, 0);


            settings.Children.Add(stackerBrakeBias);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetBrakePowerStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Brake Power", labelWidth);

            Grid settings = GetGrid(1, 90);
            Grid.SetColumn(settings, 1);


            StackPanel stackerBrakePower = new StackPanel { Orientation = Orientation.Horizontal };
            ComboBox comboBrakePower = new ComboBox() { Width = 88, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboBrakePower.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BrakePower);
            comboBrakePower.SelectedIndex = Setup.advancedSetup.mechanicalBalance.brakeTorque;
            comboBrakePower.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.brakeTorque = comboBrakePower.SelectedIndex; };
            stackerBrakePower.Children.Add(comboBrakePower);
            Grid.SetColumn(stackerBrakePower, 0);


            settings.Children.Add(stackerBrakePower);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetDiffPreloadStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Diff Preload", labelWidth);

            Grid settings = GetGrid(1, 90);
            Grid.SetColumn(settings, 1);


            StackPanel stackerPreload = new StackPanel { Orientation = Orientation.Horizontal };
            ComboBox comboPreload = new ComboBox() { Width = 88, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboPreload.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.PreloadDifferential);
            comboPreload.SelectedIndex = Setup.advancedSetup.drivetrain.preload;
            comboPreload.SelectionChanged += (s, e) => { Setup.advancedSetup.drivetrain.preload = comboPreload.SelectedIndex; };
            stackerPreload.Children.Add(comboPreload);
            Grid.SetColumn(stackerPreload, 0);


            settings.Children.Add(stackerPreload);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetAntiRollBarStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Anti roll bar", labelWidth);

            int blockWidth = 50;

            Grid settings = GetGrid(2, blockWidth + 45);
            Grid.SetColumn(settings, 1);


            // FL
            StackPanel stackerFront = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFront.Children.Add(new Label() { Content = "Front" });
            ComboBox comboFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.AntiRollBarFront);
            comboFL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.aRBFront;
            comboFL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.aRBFront = comboFL.SelectedIndex; };
            stackerFront.Children.Add(comboFL);
            Grid.SetColumn(stackerFront, 0);

            // FR
            StackPanel stackerRear = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRear.Children.Add(new Label() { Content = "Rear" });
            ComboBox comboFR = new ComboBox() { Width = blockWidth + 4, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.AntiRollBarRear);
            comboFR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.aRBRear;
            comboFR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.aRBRear = comboFR.SelectedIndex; };
            stackerRear.Children.Add(comboFR);
            Grid.SetColumn(stackerRear, 1);


            settings.Children.Add(stackerFront);
            settings.Children.Add(stackerRear);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetBumpstopRangeStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Bumpstop range", labelWidth);

            int blockWidth = 65;

            Grid settings = GetGrid(4, blockWidth + 30);
            Grid.SetColumn(settings, 1);


            // FL
            StackPanel stackerFL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFL.Children.Add(new Label() { Content = "FL" });
            ComboBox comboFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRangeFronts);
            comboFL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.FrontLeft];
            comboFL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.FrontLeft] = comboFL.SelectedIndex; };
            stackerFL.Children.Add(comboFL);
            Grid.SetColumn(stackerFL, 0);

            // FR
            StackPanel stackerFR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFR.Children.Add(new Label() { Content = "FR" });
            ComboBox comboFR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRangeFronts);
            comboFR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.FrontRight];
            comboFR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.FrontRight] = comboFR.SelectedIndex; };
            stackerFR.Children.Add(comboFR);
            Grid.SetColumn(stackerFR, 1);

            // RL
            StackPanel stackerRL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRL.Children.Add(new Label() { Content = "RL" });
            ComboBox comboRL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboRL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRangeRears);
            comboRL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.RearLeft];
            comboRL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.RearLeft] = comboRL.SelectedIndex; };
            stackerRL.Children.Add(comboRL);
            Grid.SetColumn(stackerRL, 2);

            // RR
            StackPanel stackerRR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRR.Children.Add(new Label() { Content = "RR" });
            ComboBox comboRR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboRR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRangeRears);
            comboRR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.RearRight];
            comboRR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopWindow[(int)Wheel.RearRight] = comboRR.SelectedIndex; };
            stackerRR.Children.Add(comboRR);
            Grid.SetColumn(stackerRR, 3);

            settings.Children.Add(stackerFL);
            settings.Children.Add(stackerFR);
            settings.Children.Add(stackerRL);
            settings.Children.Add(stackerRR);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetBumpstopRateStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Bumpstop rate", labelWidth);

            int blockWidth = 65;

            Grid settings = GetGrid(4, blockWidth + 30);
            Grid.SetColumn(settings, 1);


            // FL
            StackPanel stackerFL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFL.Children.Add(new Label() { Content = "FL" });
            ComboBox comboFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRate);
            comboFL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.FrontLeft];
            comboFL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.FrontLeft] = comboFL.SelectedIndex; };
            stackerFL.Children.Add(comboFL);
            Grid.SetColumn(stackerFL, 0);

            // FR
            StackPanel stackerFR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFR.Children.Add(new Label() { Content = "FR" });
            ComboBox comboFR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRate);
            comboFR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.FrontRight];
            comboFR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.FrontRight] = comboFR.SelectedIndex; };
            stackerFR.Children.Add(comboFR);
            Grid.SetColumn(stackerFR, 1);

            // RL
            StackPanel stackerRL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRL.Children.Add(new Label() { Content = "RL" });
            ComboBox comboRL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboRL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRate);
            comboRL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.RearLeft];
            comboRL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.RearLeft] = comboRL.SelectedIndex; };
            stackerRL.Children.Add(comboRL);
            Grid.SetColumn(stackerRL, 2);

            // RR
            StackPanel stackerRR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRR.Children.Add(new Label() { Content = "RR" });
            ComboBox comboRR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboRR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.BumpstopRate);
            comboRR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.RearRight];
            comboRR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.bumpStopRateUp[(int)Wheel.RearRight] = comboRR.SelectedIndex; };
            stackerRR.Children.Add(comboRR);
            Grid.SetColumn(stackerRR, 3);

            settings.Children.Add(stackerFL);
            settings.Children.Add(stackerFR);
            settings.Children.Add(stackerRL);
            settings.Children.Add(stackerRR);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetWheelRatesStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Wheelrate", labelWidth);

            int blockWidth = 65;

            Grid settings = GetGrid(4, blockWidth + 30);
            Grid.SetColumn(settings, 1);

            // FL
            StackPanel stackerFL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFL.Children.Add(new Label() { Content = "FL" });
            ComboBox comboFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.WheelRateFronts);
            comboFL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.FrontLeft];
            comboFL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.FrontLeft] = comboFL.SelectedIndex; };
            stackerFL.Children.Add(comboFL);
            Grid.SetColumn(stackerFL, 0);

            // FR
            StackPanel stackerFR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFR.Children.Add(new Label() { Content = "FR" });
            ComboBox comboFR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboFR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.WheelRateFronts);
            comboFR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.FrontRight];
            comboFR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.FrontRight] = comboFR.SelectedIndex; };
            stackerFR.Children.Add(comboFR);
            Grid.SetColumn(stackerFR, 1);

            // RL
            StackPanel stackerRL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRL.Children.Add(new Label() { Content = "RL" });
            ComboBox comboRL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboRL.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.WheelRateRears);
            comboRL.SelectedIndex = Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.RearLeft];
            comboRL.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.RearLeft] = comboRL.SelectedIndex; };
            stackerRL.Children.Add(comboRL);
            Grid.SetColumn(stackerRL, 2);

            // RR
            StackPanel stackerRR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRR.Children.Add(new Label() { Content = "RR" });
            ComboBox comboRR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboRR.ItemsSource = GetIntegerRangeCollection(SetupChanger.MechanicalSetupChanger.WheelRateRears);
            comboRR.SelectedIndex = Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.RearRight];
            comboRR.SelectionChanged += (s, e) => { Setup.advancedSetup.mechanicalBalance.wheelRate[(int)Wheel.RearRight] = comboRR.SelectedIndex; };
            stackerRR.Children.Add(comboRR);
            Grid.SetColumn(stackerRR, 3);


            settings.Children.Add(stackerFL);
            settings.Children.Add(stackerFR);
            settings.Children.Add(stackerRL);
            settings.Children.Add(stackerRR);

            grid.Children.Add(settings);

            return grid;
        }

#endregion

        #region TyreSetupChanger
        private Grid GetTyrePressureStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("PSI", labelWidth);

            int blockWidth = 50;

            Grid settings = GetGrid(4, blockWidth + 30);

            Grid.SetColumn(settings, 1);

            // FL
            StackPanel stackerFL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFL.Children.Add(new Label() { Content = "FL" });
            ComboBox comboPressureFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboPressureFL.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.TyrePressures);
            comboPressureFL.SelectedIndex = Setup.basicSetup.tyres.tyrePressure[(int)Wheel.FrontLeft];
            comboPressureFL.SelectionChanged += (s, e) => { Setup.basicSetup.tyres.tyrePressure[(int)Wheel.FrontLeft] = comboPressureFL.SelectedIndex; };
            stackerFL.Children.Add(comboPressureFL);
            Grid.SetColumn(stackerFL, 0);


            // FR
            StackPanel stackerFR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFR.Children.Add(new Label() { Content = "FR" });
            ComboBox comboPressureFR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboPressureFR.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.TyrePressures);
            comboPressureFR.SelectedIndex = Setup.basicSetup.tyres.tyrePressure[(int)Wheel.FrontRight];
            comboPressureFR.SelectionChanged += (s, e) => { Setup.basicSetup.tyres.tyrePressure[(int)Wheel.FrontRight] = comboPressureFR.SelectedIndex; };
            stackerFR.Children.Add(comboPressureFR);
            Grid.SetColumn(stackerFR, 1);

            // RL
            StackPanel stackerRL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRL.Children.Add(new Label() { Content = "RL" });
            ComboBox comboPressureRL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboPressureRL.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.TyrePressures);
            comboPressureRL.SelectedIndex = Setup.basicSetup.tyres.tyrePressure[(int)Wheel.RearLeft];
            comboPressureRL.SelectionChanged += (s, e) => { Setup.basicSetup.tyres.tyrePressure[(int)Wheel.RearLeft] = comboPressureRL.SelectedIndex; };
            stackerRL.Children.Add(comboPressureRL);
            Grid.SetColumn(stackerRL, 2);

            // RR
            StackPanel stackerRR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRR.Children.Add(new Label() { Content = "RR" });
            ComboBox comboPressureRR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboPressureRR.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.TyrePressures);
            comboPressureRR.SelectedIndex = Setup.basicSetup.tyres.tyrePressure[(int)Wheel.RearRight];
            comboPressureRR.SelectionChanged += (s, e) => { Setup.basicSetup.tyres.tyrePressure[(int)Wheel.RearRight] = comboPressureRR.SelectedIndex; };
            stackerRR.Children.Add(comboPressureRR);
            Grid.SetColumn(stackerRR, 3);


            settings.Children.Add(stackerFL);
            settings.Children.Add(stackerFR);
            settings.Children.Add(stackerRL);
            settings.Children.Add(stackerRR);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetCamberStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Camber", labelWidth);

            int blockWidth = 50;

            Grid settings = GetGrid(4, blockWidth + 30);
            Grid.SetColumn(settings, 1);

            // FL
            StackPanel stackerFL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFL.Children.Add(new Label() { Content = "FL" });
            ComboBox comboToeFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboToeFL.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.CamberFront);
            comboToeFL.SelectedIndex = Setup.basicSetup.alignment.camber[(int)Wheel.FrontLeft];
            comboToeFL.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.FrontLeft] = comboToeFL.SelectedIndex; };
            stackerFL.Children.Add(comboToeFL);
            Grid.SetColumn(stackerFL, 0);

            // FR
            StackPanel stackerFR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFR.Children.Add(new Label() { Content = "FR" });
            ComboBox comboToeFR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboToeFR.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.CamberFront);
            comboToeFR.SelectedIndex = Setup.basicSetup.alignment.camber[(int)Wheel.FrontRight];
            comboToeFR.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.FrontRight] = comboToeFR.SelectedIndex; };
            stackerFR.Children.Add(comboToeFR);
            Grid.SetColumn(stackerFR, 1);

            // RL
            StackPanel stackerRL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRL.Children.Add(new Label() { Content = "RL" });
            ComboBox comboToeRL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboToeRL.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.CamberRear);
            comboToeRL.SelectedIndex = Setup.basicSetup.alignment.camber[(int)Wheel.RearLeft];
            comboToeRL.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.RearLeft] = comboToeRL.SelectedIndex; };
            stackerRL.Children.Add(comboToeRL);
            Grid.SetColumn(stackerRL, 2);

            // RR
            StackPanel stackerRR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRR.Children.Add(new Label() { Content = "RR" });
            ComboBox comboToeRR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboToeRR.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.CamberRear);
            comboToeRR.SelectedIndex = Setup.basicSetup.alignment.camber[(int)Wheel.RearRight];
            comboToeRR.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.RearRight] = comboToeRR.SelectedIndex; };
            stackerRR.Children.Add(comboToeRR);
            Grid.SetColumn(stackerRR, 3);

            settings.Children.Add(stackerFL);
            settings.Children.Add(stackerFR);
            settings.Children.Add(stackerRL);
            settings.Children.Add(stackerRR);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetCasterStacker(int labelWidth)
        {
            // Caster inputs 
            Grid grid = GetMainGrid("Caster", labelWidth);

            int blockWidth = 50;

            Grid settings = GetGrid(2, blockWidth + 30);
            Grid.SetColumn(settings, 1);

            // LF
            StackPanel stackerCasterLF = new StackPanel() { Orientation = Orientation.Horizontal };
            stackerCasterLF.Children.Add(new Label() { Content = "FL" });
            ComboBox comboCasterLF = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboCasterLF.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.Caster);
            comboCasterLF.SelectedIndex = Setup.basicSetup.alignment.casterLF;
            comboCasterLF.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.casterLF = comboCasterLF.SelectedIndex; };
            stackerCasterLF.Children.Add(comboCasterLF);
            Grid.SetColumn(stackerCasterLF, 0);

            // RF
            StackPanel stackerCasterRF = new StackPanel() { Orientation = Orientation.Horizontal };
            stackerCasterRF.Children.Add(new Label() { Content = "FR" });
            ComboBox comboCasterRF = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboCasterRF.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.Caster);
            comboCasterRF.SelectedIndex = Setup.basicSetup.alignment.casterRF;
            comboCasterRF.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.casterRF = comboCasterRF.SelectedIndex; };
            stackerCasterRF.Children.Add(comboCasterRF);
            Grid.SetColumn(stackerCasterRF, 1);

            settings.Children.Add(stackerCasterLF);
            settings.Children.Add(stackerCasterRF);

            grid.Children.Add(settings);

            return grid;
        }

        private Grid GetToeStacker(int labelWidth)
        {
            Grid grid = GetMainGrid("Toe", labelWidth);

            int blockWidth = 50;

            Grid settings = GetGrid(4, blockWidth + 30);
            Grid.SetColumn(settings, 1);

            // FL
            StackPanel stackerFL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFL.Children.Add(new Label() { Content = "FL" });
            ComboBox comboCasterFL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboCasterFL.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.ToeFront);
            comboCasterFL.SelectedIndex = Setup.basicSetup.alignment.toe[(int)Wheel.FrontLeft];
            comboCasterFL.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.FrontLeft] = comboCasterFL.SelectedIndex; };
            stackerFL.Children.Add(comboCasterFL);
            Grid.SetColumn(stackerFL, 0);

            // FR
            StackPanel stackerFR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerFR.Children.Add(new Label() { Content = "FR" });
            ComboBox comboCasterFR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboCasterFR.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.ToeFront);
            comboCasterFR.SelectedIndex = Setup.basicSetup.alignment.toe[(int)Wheel.FrontRight];
            comboCasterFR.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.FrontRight] = comboCasterFR.SelectedIndex; };
            stackerFR.Children.Add(comboCasterFR);
            Grid.SetColumn(stackerFR, 1);

            // RL
            StackPanel stackerRL = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRL.Children.Add(new Label() { Content = "RL" });
            ComboBox comboCasterRL = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right }; ;
            comboCasterRL.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.ToeRear);
            comboCasterRL.SelectedIndex = Setup.basicSetup.alignment.toe[(int)Wheel.RearLeft];
            comboCasterRL.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.RearLeft] = comboCasterRL.SelectedIndex; };
            stackerRL.Children.Add(comboCasterRL);
            Grid.SetColumn(stackerRL, 2);

            // RR
            StackPanel stackerRR = new StackPanel { Orientation = Orientation.Horizontal };
            stackerRR.Children.Add(new Label() { Content = "RR" });
            ComboBox comboCasterRR = new ComboBox() { Width = blockWidth, HorizontalContentAlignment = HorizontalAlignment.Right };
            comboCasterRR.ItemsSource = GetDoubleRangeCollection(SetupChanger.TyreSetupChanger.ToeRear);
            comboCasterRR.SelectedIndex = Setup.basicSetup.alignment.toe[(int)Wheel.RearRight];
            comboCasterRR.SelectionChanged += (s, e) => { Setup.basicSetup.alignment.camber[(int)Wheel.RearRight] = comboCasterRR.SelectedIndex; };
            stackerRR.Children.Add(comboCasterRR);
            Grid.SetColumn(stackerRR, 3);


            settings.Children.Add(stackerFL);
            settings.Children.Add(stackerFR);
            settings.Children.Add(stackerRL);
            settings.Children.Add(stackerRR);

            grid.Children.Add(settings);

            return grid;
        }
        #endregion

        private Grid GetMainGrid(string label, int labelWidth)
        {
            Grid grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                ColumnDefinitions = {
                    new ColumnDefinition() {  Width = new GridLength(labelWidth) },
                    new ColumnDefinition()
                }
            };
            grid.Children.Add(new Label() { Content = label });

            return grid;
        }

        private Grid GetGrid(int columnCount, int columnWidth)
        {
            Grid customGrid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            for (int i = 0; i < columnCount; i++)
            {
                customGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });
            }

            return customGrid;
        }

        private double[] GetDoubleRangeCollection(SetupDoubleRange doubleRange)
        {
            if (doubleRange.LUT != null)
            {
                return doubleRange.LUT;
            }

            List<double> collection = new List<double>();

            for (double i = doubleRange.Min; i < doubleRange.Max + doubleRange.Increment; i += doubleRange.Increment)
            {
                collection.Add(Math.Round(i, 2));
            }

            return collection.ToArray();
        }

        private int[] GetIntegerRangeCollection(SetupIntRange intRange)
        {
            if (intRange.LUT != null)
            {
                return intRange.LUT;
            }

            List<int> collection = new List<int>();

            for (int i = intRange.Min; i < intRange.Max + intRange.Increment; i += intRange.Increment)
            {
                collection.Add(i);
            }

            return collection.ToArray();
        }

        public Root GetSetup(FileInfo file)
        {
            if (!file.Exists)
                return null;

            string jsonString = string.Empty;
            try
            {
                using (FileStream fileStream = file.OpenRead())
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        jsonString = reader.ReadToEnd();
                        reader.Close();
                        fileStream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            Root setup = JsonConvert.DeserializeObject<Root>(jsonString);
            return setup;
        }
    }
}
