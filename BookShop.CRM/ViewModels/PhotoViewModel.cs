using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class PhotoViewModel : BaseViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        public PhotoViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            List<PhotoModel> photos = new();
            photos.Add(new PhotoModel { });
            Photos = photos;
            SelectedPhoto = new(Photos[0]);

            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
            ShowPhotoCommand = new Command(param => true, ExecuteShowPhoto);
            UploadPhotoCommand = new Command(param => true, ExecuteUploadPhoto);
        }

        public async void ExecuteLoad(object param)
        {
            LoadEnabled = false;

            try
            {
                int selected_id = selectedPhoto.Model.Id;
                List<PhotoModel> photos = (await unitOfWork.PhotoRepository.GetAll()).ToList();
                photos.Reverse();
                photos.Insert(0, new PhotoModel { });
                Photos = new(photos);
                SelectedPhoto = new(Photos.FirstOrDefault(p => p.Id == selected_id));
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LoadEnabled = true;
        }

        public bool CanAdd(object param)
        {
            return !SelectedPhoto.HasErrors;
        }

        public async void ExecuteAdd(object param)
        {
            AddEnabled = false;

            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("You must add photo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    AddEnabled = true;
                    return;
                }

                PhotoModel photo = await unitOfWork.PhotoRepository.Add(selectedPhoto.Model);
                if(photo != null)
                {
                    await unitOfWork.PhotoRepository.UploadFile(FilePath, photo.Id);
                    List<PhotoModel> photos = Photos;
                    photos[0] = photo;
                    photos.Insert(0, new PhotoModel { });
                    Photos = new(photos);
                    SelectedPhoto = new(Photos[1]);
                }

            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AddEnabled = true;
        }

        public async void ExecuteUpdate(object param)
        {
            UpdateEnabled = false;

            try
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedPhoto.Id}",
                "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if(result == MessageBoxResult.Yes)
                {
                    int selected_id = selectedPhoto.Model.Id;
                    PhotoModel photo = await unitOfWork.PhotoRepository.Update(selectedPhoto.Model);
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        await unitOfWork.PhotoRepository.UploadFile(FilePath, photo.Id);
                    }
                    List<PhotoModel> photos = Photos;
                    PhotoModel old = photos.FirstOrDefault(p => p.Id == selected_id);
                    int index = photos.IndexOf(old);
                    photos[index] = photo;
                    Photos = new(photos);
                    SelectedPhoto = new(Photos[index]);
                }

            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateEnabled = true;
        }

        public async void ExecuteDelete(object param)
        {
            DeleteEnabled = false;

            try
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedPhoto.Id}",
                "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if(result == MessageBoxResult.Yes)
                {
                    int selected_id = selectedPhoto.Model.Id;
                    await unitOfWork.PhotoRepository.Remove(selected_id);
                    List<PhotoModel> photos = Photos;
                    PhotoModel photo = photos.FirstOrDefault(p => p.Id == selected_id);
                    photos.Remove(photo);
                    Photos = new(photos);
                    SelectedPhoto = new(Photos[0]);
                }

            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DeleteEnabled = true;
        }

        public Action<List<byte>> ShowPhoto { get; set; }

        public async void ExecuteShowPhoto(object param)
        {
            try
            {
                List<byte> bytes = (await unitOfWork.PhotoRepository.GetPhotoBytes(selectedPhoto.Model.Id)).ToList();
                ShowPhoto(bytes);
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string filePath;
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public void ExecuteUploadPhoto(object param)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Png Files|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        public ICommand LoadCommand { get; }

        public ICommand AddCommand { get; }

        public ICommand UpdateCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand ShowPhotoCommand { get; }

        public ICommand UploadPhotoCommand { get; }


        private List<PhotoModel> photos;
        public List<PhotoModel> Photos
        {
            get => photos;
            set
            {
                photos = value;
                OnPropertyChanged(nameof(Photos));
            }
        }

        private PhotoWrapper selectedPhoto;
        public PhotoWrapper SelectedPhoto
        {
            get => selectedPhoto;
            set
            {
                selectedPhoto = value;

                if(selectedPhoto?.Model?.Id == 0)
                {
                    ShowPhotoCommandVisibility = Visibility.Hidden;
                    AddVisibility = Visibility.Visible;
                    UpdateVisibility = Visibility.Hidden;
                    DeleteVisibility = Visibility.Hidden;
                }
                else
                {
                    ShowPhotoCommandVisibility = Visibility.Visible;
                    AddVisibility = Visibility.Hidden;
                    UpdateVisibility = Visibility.Visible;
                    DeleteVisibility = Visibility.Visible;
                }
                FilePath = string.Empty;
                OnPropertyChanged(nameof(SelectedPhoto));
            }
        }

        private Visibility showPhotoCommandVisibility = Visibility.Hidden;
        public Visibility ShowPhotoCommandVisibility
        {
            get => showPhotoCommandVisibility;
            set
            {
                showPhotoCommandVisibility = value;
                OnPropertyChanged(nameof(ShowPhotoCommandVisibility));
            }
        }

        private Visibility deleteVisibility = Visibility.Visible;
        public Visibility DeleteVisibility
        {
            get => deleteVisibility;
            set
            {
                deleteVisibility = value;
                OnPropertyChanged(nameof(DeleteVisibility));
            }
        }

        private Visibility updateVisibility = Visibility.Visible;
        public Visibility UpdateVisibility
        {
            get => updateVisibility;
            set
            {
                updateVisibility = value;
                OnPropertyChanged(nameof(UpdateVisibility));
            }
        }

        private Visibility addVisibility = Visibility.Visible;
        public Visibility AddVisibility
        {
            get => addVisibility;
            set
            {
                addVisibility = value;
                OnPropertyChanged(nameof(AddVisibility));
            }
        }

        private bool deleteEnabled = true;
        public bool DeleteEnabled
        {
            get => deleteEnabled;
            set
            {
                deleteEnabled = value;
                OnPropertyChanged(nameof(DeleteEnabled));
            }
        }

        private bool updateEnabled = true;
        public bool UpdateEnabled
        {
            get => updateEnabled;
            set
            {
                updateEnabled = value;
                OnPropertyChanged(nameof(UpdateEnabled));
            }
        }

        private bool addEnabled = true;
        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                addEnabled = value;
                OnPropertyChanged(nameof(AddEnabled));
            }
        }

        private bool loadEnabled = true;
        public bool LoadEnabled
        {
            get => loadEnabled;
            set
            {
                loadEnabled = value;
                OnPropertyChanged(nameof(LoadEnabled));
            }
        }
    }
}
