using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LC_Points.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Popups;
using LC_Points.Services;

namespace LC_Points.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IRepository<ScoreModel> _repository = App.ScoresRepository; 
     
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

            //call methods to initilise list data
            InitSubjectTypes();
            InitOrdinaryGradePairs();
            InitHigherGradePairs();
            InitHigherMathGradePairs();
        }


        public List<ScoreModel> Subjects { get; set; }
        public List<StringKeyValue> HigherGradePointKV { get; set; }
        public List<StringKeyValue> OrdinaryGradePointKV { get; set; }
        public List<StringKeyValue> HigherMathGradePointKV { get; set; }




        public ObservableCollection<ScoreModel> AddedSubjectGradePairs
        {
            get { return _repository.Collection; }
        }

        

        private ScoreModel _selectedSubject;
        public ScoreModel SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                if (value != _selectedSubject)
                {
                    _selectedSubject = value;
                    RaisePropertyChanged("SelectedSubject");
                    RaisePropertyChanged("ButtonEnabled");

                }
                
            }
        }

            

        private StringKeyValue _selectedHigherGrade;
        public StringKeyValue SelectedHigherGrade
        {
            get { return _selectedHigherGrade; }
            set
            {
                if (value != _selectedHigherGrade)
                {
                    _selectedHigherGrade = value;
                    RaisePropertyChanged("SelectedHigherGrade");
                    RaisePropertyChanged("ButtonEnabled");


                }
               
            }
        }


        private StringKeyValue _selectedHigherMathGrade;
        public StringKeyValue SelectedHigherMathGrade
        {
            get { return _selectedHigherMathGrade; }
            set
            {
                if (value != _selectedHigherMathGrade)
                {
                    _selectedHigherMathGrade = value;
                    RaisePropertyChanged("SelectedHigherMathGrade");
                    RaisePropertyChanged("ButtonEnabled");


                }

            }
        }



        private StringKeyValue _selectedOrdinaryGrade;
        public StringKeyValue SelectedOrdinaryGrade
        {
            get { return _selectedOrdinaryGrade; }
            set
            {
                if (value != _selectedOrdinaryGrade)
                {
                    _selectedOrdinaryGrade = value;
                    RaisePropertyChanged("SelectedOrdinaryGrade");
                    RaisePropertyChanged("ButtonEnabled");

                }
               
            }
        }

        

        private int _totalPoints;
        public int TotalPoints
        {
            get { return _totalPoints; }
            set
            {
                if (value != _totalPoints)
                {
                    _totalPoints = value;
                    RaisePropertyChanged("TotalPoints");
                }
            }
        }



        //button enabled bool
        public bool ButtonEnabled
        { 
            get 
            {
                return SelectedSubject != null && (SelectedOrdinaryGrade != null || SelectedHigherGrade != null || SelectedHigherMathGrade != null); 
            } 
        }

    
        //Higher math toggle button bool
        private bool _isHigherMath;
        public bool IsHigherMath
        {
            get
            {
                return _isHigherMath;
            }
            set
            {
                _isHigherMath = value;
                RaisePropertyChanged("IsHigherMath");
                RaisePropertyChanged("IsEitherHigherVisible");
            }
        }


        
        //Higher toggle button bool property
        private bool _isHigher;
        public bool IsHigher
        {
            get
            {
                return _isHigher;
            }
            set
            {
                _isHigher = value;
                RaisePropertyChanged("IsHigher");
                RaisePropertyChanged("IsEitherHigherVisible");
            }
        }




        //Method to store Subject and Grade from Combo Boxes
        public void AddSubjectAndGrade()
        {

            string SelectedSubjectName = "null subject";
            int SelectedPoints = 0;


            SelectedSubjectName = SelectedSubject.Subject;           
 
            SelectedPoints = IsHigher ? SelectedHigherGrade.Value : SelectedOrdinaryGrade.Value;



            if (_repository.Count <= 5 && _repository.Collection.All(s => s.Subject != SelectedSubjectName)) 
            {
                _repository.Add(new ScoreModel() {Subject = SelectedSubjectName, Points = SelectedPoints});
                CalculateLeavingCertPoints();
            }

            
        }

       
        //Method to calculate the points result
        private void CalculateLeavingCertPoints()
        {

           //Logic:
            //IF 6 subjects and grades
            //add 6 grade points
            //output result of addition
            TotalPoints = _repository.Collection.Sum(x => x.Points);

        }



        RelayCommand viewGradesCommand;
        public RelayCommand ViewGradesCommand
        {
            get
            {
                if (viewGradesCommand == null)
                {
                    viewGradesCommand = new RelayCommand(() =>
                    {
                        //do something...
                    });
                }
                return viewGradesCommand;
            }
        }


        
        RelayCommand addGradeCommand;
        public RelayCommand AddGradeCommand
        {
            get
            {
                if (addGradeCommand == null)
                {
                    addGradeCommand = new RelayCommand(() =>
                    {
                        AddSubjectAndGrade();

                    });
                }
                return addGradeCommand;
            }
        }





        RelayCommand clearGradesCommand;
        public RelayCommand ClearGradesCommand
        {
            get
            {
                if (clearGradesCommand == null)
                {
                    clearGradesCommand = new RelayCommand(() =>
                    {
                        //call to empty collection items
                        _repository.Clear();
                        TotalPoints = 0;

                    });
                }
                return clearGradesCommand;
            }
        }
         


        public class StringKeyValue
        {
            public string Key { get; set; }
            public int Value { get; set; }
        }


        public void InitOrdinaryGradePairs()
        {

            List<StringKeyValue> ordinaryGradePointKVTemp = new List<StringKeyValue>();


            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "A1", Value = 60 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "A2", Value = 50 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "B1", Value = 45 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "B2", Value = 40 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "B3", Value = 35 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "C1", Value = 30 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "C2", Value = 25 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "C3", Value = 20 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "D1", Value = 15 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "D2", Value = 10 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "D3", Value = 5 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "E,F,NG", Value = 0 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "Pass", Value = 30 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "Merit", Value = 50 });
            ordinaryGradePointKVTemp.Add(new StringKeyValue { Key = "Distinction", Value = 70 });


            OrdinaryGradePointKV = ordinaryGradePointKVTemp;

        }


        public void InitHigherGradePairs()
        {

            List<StringKeyValue> higherGradePointKVTemp = new List<StringKeyValue>();


            higherGradePointKVTemp.Add(new StringKeyValue { Key = "A1", Value = 100 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "A2", Value = 90 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "B1", Value = 85 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "B2", Value = 80 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "B3", Value = 75 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "C1", Value = 70 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "C2", Value = 65 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "C3", Value = 60 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "D1", Value = 55 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "D2", Value = 50 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "D3", Value = 45 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "E,F,NG", Value = 0 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "Pass", Value = 30 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "Merit", Value = 50 });
            higherGradePointKVTemp.Add(new StringKeyValue { Key = "Distinction", Value = 70 });


            HigherGradePointKV = higherGradePointKVTemp;
        }



        public void InitHigherMathGradePairs()
        {

            List<StringKeyValue> higherMathGradePointKVTemp = new List<StringKeyValue>();


            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "A1", Value = 125 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "A2", Value = 115 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "B1", Value = 110 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "B2", Value = 105 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "B3", Value = 100 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "C1", Value = 95 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "C2", Value = 90 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "C3", Value = 85 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "D1", Value = 80 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "D2", Value = 75 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "D3", Value = 70 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "E,F,NG", Value = 0 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "Pass", Value = 30 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "Merit", Value = 50 });
            higherMathGradePointKVTemp.Add(new StringKeyValue { Key = "Distinction", Value = 70 });


            HigherMathGradePointKV = higherMathGradePointKVTemp;
        }



        public void InitSubjectTypes()
        {
            List<ScoreModel> subjectList = new List<ScoreModel>();

            // Adding Subjects to List
            subjectList.Add(new ScoreModel { Subject = "Accounting" });
            subjectList.Add(new ScoreModel { Subject = "Agricultural Economics" });
            subjectList.Add(new ScoreModel { Subject = "Agricultural Science" });
            subjectList.Add(new ScoreModel { Subject = "Ancient Greek" });
            subjectList.Add(new ScoreModel { Subject = "Applied Math" });
            subjectList.Add(new ScoreModel { Subject = "Arabic" });
            subjectList.Add(new ScoreModel { Subject = "Art" });
            subjectList.Add(new ScoreModel { Subject = "Artistic & Creative Group" });
            subjectList.Add(new ScoreModel { Subject = "Biology" });
            subjectList.Add(new ScoreModel { Subject = "Business" });
            subjectList.Add(new ScoreModel { Subject = "Business Group" });
            subjectList.Add(new ScoreModel { Subject = "Chemistry" });
            subjectList.Add(new ScoreModel { Subject = "Classical Studies" });
            subjectList.Add(new ScoreModel { Subject = "Construction Studies" });
            subjectList.Add(new ScoreModel { Subject = "Design & Comm Graphics" });
            subjectList.Add(new ScoreModel { Subject = "Economics" });
            subjectList.Add(new ScoreModel { Subject = "Engineering" });
            subjectList.Add(new ScoreModel { Subject = "English" });
            subjectList.Add(new ScoreModel { Subject = "French" });
            subjectList.Add(new ScoreModel { Subject = "Geography" });
            subjectList.Add(new ScoreModel { Subject = "German" });
            subjectList.Add(new ScoreModel { Subject = "Hebrew Studies" });
            subjectList.Add(new ScoreModel { Subject = "History" });
            subjectList.Add(new ScoreModel { Subject = "Home Economics" });
            subjectList.Add(new ScoreModel { Subject = "Irish" });
            subjectList.Add(new ScoreModel { Subject = "Italian" });
            subjectList.Add(new ScoreModel { Subject = "Japanese" });
            subjectList.Add(new ScoreModel { Subject = "Languages & Humanities" });
            subjectList.Add(new ScoreModel { Subject = "Latin" });
            subjectList.Add(new ScoreModel { Subject = "Link Modules" });
            subjectList.Add(new ScoreModel { Subject = "Mathematics" });
            subjectList.Add(new ScoreModel { Subject = "Music" });
            subjectList.Add(new ScoreModel { Subject = "Other Language" });
            subjectList.Add(new ScoreModel { Subject = "Physics" });
            subjectList.Add(new ScoreModel { Subject = "Physics & Chemistry" });
            subjectList.Add(new ScoreModel { Subject = "Practical Group" });
            subjectList.Add(new ScoreModel { Subject = "Religious Education" });
            subjectList.Add(new ScoreModel { Subject = "Russian" });
            subjectList.Add(new ScoreModel { Subject = "Science Group" });
            subjectList.Add(new ScoreModel { Subject = "Social Group" });
            subjectList.Add(new ScoreModel { Subject = "Spanish" });
            subjectList.Add(new ScoreModel { Subject = "Technology" });

            Subjects = subjectList;

        }

    }
}