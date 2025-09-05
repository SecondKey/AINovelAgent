using System;
using System.Windows;
using System.Windows.Input;

namespace AINovelAgent.Commands.Routing
{
    public static class CommandBindingsProvider
    {
        public static void Attach(Window window, CommandRouter router, params ICommand[] commands)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (router == null) throw new ArgumentNullException(nameof(router));

            foreach (var cmd in commands)
            {
                var binding = new CommandBinding(cmd);
                binding.CanExecute += (s, e) =>
                {
                    e.CanExecute = router.CanExecute(e.Command, e.Parameter);
                    e.Handled = true;
                };
                binding.Executed += async (s, e) =>
                {
                    await router.ExecuteAsync(e.Command, e.Parameter);
                    e.Handled = true;
                };
                window.CommandBindings.Add(binding);
            }
        }
    }
}


