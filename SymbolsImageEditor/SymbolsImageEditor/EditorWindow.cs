using System;
using System.IO;
using System.Windows.Forms;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Button = SadConsole.Controls.Button;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace SymbolsImageEditor
{
    public class EditorWindow : ControlsConsole
    {
        private const string SymbolsImageFileFilter = "Symbols Image|*.simg|All|*.*";
        private const string SymbolsImageFileExtension = ".simg";
        private const int DefaultImageWidth = 20;
        private const int DefaultImageHeight = 5;

        private ColorDialog colorPicker;

        private readonly Brush brush;
        private SymbolsImage image;
        private DrawingSurface imageControl;
        private System.Drawing.Color backgroundColor = System.Drawing.Color.Black;

        public EditorWindow()
            : base(Program.Width, Program.Height)
        {
            IsFocused = true;
            UseKeyboard = true;
            brush = new Brush();
            image = new SymbolsImage(DefaultImageWidth, DefaultImageHeight);

            DefaultBackground = Color.Black;
            DefaultForeground = Color.White;

            Fill(Color.White, Color.Black, null);

            colorPicker = new ColorDialog();

            InitializeControls();
        }

        private void InitializeControls()
        {
            var foreColorButton = new Button(1)
            {
                Position = new Point(1, 2),
                CanFocus = false,
                Text = "Q"
            };
            foreColorButton.Click += foreColorButton_Click;
            Add(foreColorButton);

            var clearForeColorButton = new Button(1)
            {
                Position = new Point(2, 2),
                CanFocus = false,
                Text = "C"
            };
            clearForeColorButton.Click += clearForeColorButton_Click;
            Add(clearForeColorButton);

            var backColorButton = new Button(1)
            {
                Position = new Point(1, 3),
                CanFocus = false,
                Text = "Q"
            };
            backColorButton.Click += backColorButton_Click;
            Add(backColorButton);

            var clearBackColorButton = new Button(1)
            {
                Position = new Point(2, 3),
                CanFocus = false,
                Text = "C"
            };
            clearBackColorButton.Click += clearBackColorButton_Click;
            Add(clearBackColorButton);

            var newImageButton = new Button(6)
            {
                Position = new Point(1, 11),
                Text = "New",
                CanFocus = false
            };
            newImageButton.Click += newImageButton_Click;
            Add(newImageButton);

            var setBackgroundColor = new Button(6)
            {
                Position = new Point(1, 5),
                Text = "Back",
                CanFocus = false
            };
            setBackgroundColor.Click += setBackgroundColor_Click;
            Add(setBackgroundColor);

            var loadImageButton = new Button(6)
            {
                Position = new Point(1, 7),
                Text = "Load",
                CanFocus = false
            };
            loadImageButton.Click += loadImageButton_Click;
            Add(loadImageButton);

            var saveImageButton = new Button(6)
            {
                Position = new Point(1, 9),
                Text = "Save",
                CanFocus = false
            };
            saveImageButton.Click += saveImageButton_Click;
            Add(saveImageButton);

            InitializeImageControl();
        }

        private void newImageButton_Click(object sender, EventArgs e)
        {
            image = new SymbolsImage(DefaultImageWidth, DefaultImageHeight);
        }

        private void setBackgroundColor_Click(object sender, EventArgs e)
        {
            colorPicker.Color = backgroundColor;
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                backgroundColor = colorPicker.Color;
            }
        }

        private void DrawImage(DrawingSurface control)
        {
            control.Surface.Fill(0, 0, Width, Color.Gray, Color.Black, '#');
            control.Surface.Fill(0, Height - 1, Width, Color.Gray, Color.Black, '#');
            control.Surface.DrawImage(1, 1, image, Color.White, ColorHelper.ConvertToXna(backgroundColor));
        }

        private void saveImageButton_Click(object sender, EventArgs args)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = SymbolsImageFileFilter,
                DefaultExt = SymbolsImageFileExtension
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (var fileStream = File.Create(saveDialog.FileName))
                {
                    SymbolsImage.SaveToFile(image, fileStream);
                }
            }
        }

        private void InitializeImageControl()
        {
            if (imageControl != null)
            {
                imageControl.MouseButtonClicked -= imageControl_MouseButtonClicked;
                Remove(imageControl);
            }
            imageControl = new DrawingSurface(image.Width + 2, image.Height + 2)
            {
                Position = new Point(30, 1),
                Theme = new DrawingSurfaceTheme
                {
                    Colors = new Colors
                    {
                        Appearance_ControlNormal = new Cell(Color.White, Color.Black)
                    }
                },
                OnDraw = DrawImage
            };
            imageControl.MouseButtonClicked += imageControl_MouseButtonClicked;
            Add(imageControl);
        }

        private void imageControl_MouseButtonClicked(object sender, SadConsole.Input.MouseEventArgs args)
        {
            var position = args.MouseState.CellPosition;
            var relativeX = position.X - imageControl.Position.X - 1;
            var relativeY = position.Y - imageControl.Position.Y - 1;

            if (relativeX < 0 || relativeX >= image.Width || relativeY < 0 || relativeY >= image.Height)
                return;

            var pixel = image[relativeX, relativeY];
            pixel.Symbol = brush.Symbol;
            pixel.Color = brush.ForeColor;
            pixel.BackgroundColor = brush.BackColor;
        }

        private void loadImageButton_Click(object sender, EventArgs args)
        {
            var browseDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = SymbolsImageFileExtension,
                Multiselect = false,
                Filter = SymbolsImageFileFilter
            };
            if (browseDialog.ShowDialog() == DialogResult.OK)
            {
                using (var fileStream = File.OpenRead(browseDialog.FileName))
                {
                    image = SymbolsImage.LoadFromFile(fileStream);
                }

                InitializeImageControl();
            }
        }

        private void foreColorButton_Click(object sender, EventArgs args)
        {
            if (brush.ForeColor.HasValue)
            {
                colorPicker.Color = brush.ForeColor.Value;
            }
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                brush.ForeColor = colorPicker.Color;
            }
        }

        private void backColorButton_Click(object sender, EventArgs args)
        {
            if (brush.BackColor.HasValue)
            {
                colorPicker.Color = brush.BackColor.Value;
            }
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                brush.BackColor = colorPicker.Color;
            }
        }

        private void clearForeColorButton_Click(object sender, EventArgs args)
        {
            brush.ForeColor = null;
        }

        private void clearBackColorButton_Click(object sender, EventArgs args)
        {
            brush.BackColor = null;
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Insert))
            {
                if (!Clipboard.ContainsText())
                    return false;

                var clipboardData = Clipboard.GetText();
                if (clipboardData.Length == 0 || clipboardData.Length > 1)
                    return false;

                brush.Symbol = clipboardData[0];
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Delete))
            {
                brush.Symbol = null;
                return true;
            }

            if (keyboard.KeysPressed.Count == 1)
            {
                var symbol = keyboard.KeysPressed[0].Character;
                brush.Symbol = symbol;
                return true;
            }

            return base.ProcessKeyboard(keyboard);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            DrawBrush();
        }

        private void DrawBrush()
        {
            const int dX = 4;

            Print(1, 0, "Brush:");
            Print(dX, 1, "Symbol: ");
            if (brush.Symbol.HasValue)
            {
                Fill(dX + 8, 1, 5, Color.White, Color.Black, ' ');
                Print(dX + 8, 1, new ColoredGlyph(Glyphs.GetGlyph(brush.Symbol.Value)));
            }
            else
            {
                Fill(dX + 8, 1, 5, Color.White, Color.Black, ' ');
                Print(dX + 8, 1, "null");
            }

            Print(dX, 2, "Fore: ");
            if (brush.ForeColor.HasValue)
            {
                Fill(dX + 6, 2, 5, Color.White, Color.Black, ' ');
                Print(dX + 6, 2, new ColoredGlyph(' ', Color.White, ColorHelper.ConvertToXna(brush.ForeColor.Value)));
            }
            else
            {
                Fill(dX + 6, 2, 5, Color.White, Color.Black, ' ');
                Print(dX + 6, 2, "null");
            }

            Print(dX, 3, "Back: ");
            if (brush.BackColor.HasValue)
            {
                Fill(dX + 6, 3, 5, Color.White, Color.Black, ' ');
                Print(dX + 6, 3, new ColoredGlyph(' ', Color.White, ColorHelper.ConvertToXna(brush.BackColor.Value)));
            }
            else
            {
                Fill(dX + 6, 3, 5, Color.White, Color.Black, ' ');
                Print(dX + 6, 3, "null");
            }
        }
    }

    public class Brush
    {
        public char? Symbol { get; set; }

        public System.Drawing.Color? ForeColor { get; set; }

        public System.Drawing.Color? BackColor { get; set; }
    }
}