using System.Collections.Generic;
using System.Windows;
using System.Windows.Interactivity;
using TriggerBase = System.Windows.Interactivity.TriggerBase;
using TriggerCollection = System.Windows.Interactivity.TriggerCollection;

namespace CoApp.Gui.Toolkit.Support
{
    public class Behaviors : List<Behavior>
    {
    }

    public class Triggers : List<TriggerBase>
    {
    }

    /// <summary>
    /// From: http://stackoverflow.com/questions/1647815/how-to-add-a-blend-behavior-in-a-style-setter
    /// </summary>
    public static class SupplementaryInteraction
    {
        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("Behaviors", typeof (Behaviors), typeof (SupplementaryInteraction),
                                                new UIPropertyMetadata(null, OnPropertyBehaviorsChanged));

        public static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached("Triggers", typeof (Triggers), typeof (SupplementaryInteraction),
                                                new UIPropertyMetadata(null, OnPropertyTriggersChanged));

        public static Behaviors GetBehaviors(DependencyObject obj)
        {
            return (Behaviors) obj.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject obj, Behaviors value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        private static void OnPropertyBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BehaviorCollection behaviors = Interaction.GetBehaviors(d);
            foreach (Behavior behavior in e.NewValue as Behaviors) behaviors.Add(behavior);
        }

        public static Triggers GetTriggers(DependencyObject obj)
        {
            return (Triggers) obj.GetValue(TriggersProperty);
        }

        public static void SetTriggers(DependencyObject obj, Triggers value)
        {
            obj.SetValue(TriggersProperty, value);
        }

        private static void OnPropertyTriggersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TriggerCollection triggers = Interaction.GetTriggers(d);
            foreach (TriggerBase trigger in e.NewValue as Triggers) triggers.Add(trigger);
        }
    }
}