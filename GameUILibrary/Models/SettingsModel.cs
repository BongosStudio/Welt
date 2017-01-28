using EmptyKeys.UserInterface.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Models
{
    public class SettingsModel : PauseViewModel
    {
        private int m_MasterVol = 100;
        private int m_MusicVol = 100;
        private int m_EffectVol = 100;
        public int MasterVolume
        {
            get { return m_MasterVol; }
            set
            {
                if (m_MasterVol == value) return;
                m_MasterVol = value;
                RaisePropertyChanged();
            }
        }

        public int MusicVolume
        {
            get { return m_MusicVol; }
            set
            {
                if (m_MusicVol == value) return;
                m_MusicVol = value;
                RaisePropertyChanged();
            }
        }
        public int EffectVolume
        {
            get { return m_EffectVol; }
            set
            {
                if (m_EffectVol == value) return;
                m_EffectVol = value;
                RaisePropertyChanged();
            }
        }

        private double m_HorizontalSens = 0.8;
        private double m_VerticalSens = 0.8;
        public double HorizontalSens
        {
            get { return m_HorizontalSens; }
            set
            {
                if (m_HorizontalSens == value) return;
                m_HorizontalSens = value;
                RaisePropertyChanged();
            }
        }
        public double VerticalSens
        {
            get { return m_VerticalSens; }
            set
            {
                if (m_VerticalSens == value) return;
                m_VerticalSens = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFullScreen { get; set; } = true;

        public ICommand ExitCommand { get; set; }

    }
}
