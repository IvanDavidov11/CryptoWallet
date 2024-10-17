import React, { useState } from 'react'
import SpinnerLoader from '../SpinnerLoader';

const FileUpload = ({ uploadCaption, onFileUpload, setUploadFormatFailed, setBadCoins, setGoodCoins, setIsLoading, isLoading }) => {
    const [file, setFile] = useState(null);
    const upload_ApiUrl = "https://localhost:7038/api/coins/upload";

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleDragOver = (event) => {
        event.preventDefault();
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (file) {
            const formData = new FormData();
            formData.append('file', file);

            try {
                setIsLoading(true);
                const response = await fetch(upload_ApiUrl, {
                    method: 'POST',
                    body: formData,
                });

                if (!response.ok) throw new Error('File upload failed');

                if (response.status === 206) {
                    const result = await response.json();
                    setBadCoins(result.badCoins);
                    setGoodCoins(result.goodCoins);
                    setUploadFormatFailed(true);
                    console.log(`${response.status} - ${result.badCoins}`);
                }
                else if (response.ok) {
                    const result = await response.text();
                    console.log(result);
                    onFileUpload();
                }

            } catch (error) {
                console.error('Error uploading file:', error);
            }
            finally {
                setIsLoading(false);
            }
        }
    };

    const handleDrop = (event) => {
        event.preventDefault();
        const droppedFile = event.dataTransfer.files[0];
        if (droppedFile) {
            setFile(droppedFile);
        }
    };

    return (
        <div>
            <h1 className="title">{uploadCaption}</h1>
            <p className="subtitle">
                Simply drag and drop your file to instantly calculate the value of your crypto portfolio.
            </p>
            <form
                className="upload-box"
                onSubmit={handleSubmit}
                onDrop={handleDrop}
                onDragOver={handleDragOver}
            >
                {isLoading ? (
                    <SpinnerLoader />
                ) : (
                    <>
                        <label htmlFor="file-upload" className="drag-drop-text">
                            <strong>Drag & Drop</strong> <br /> or
                            <a href="#" onClick={() => document.getElementById('file-upload').click()}><strong> browse</strong></a> your file
                        </label>
                        <input
                            type="file"
                            id="file-upload"
                            accept=".txt, .csv" // Accepts text and CSV files
                            onChange={handleFileChange}
                            style={{ display: 'none' }}
                        />
                        <button
                            disabled={!file}
                            type='submit'
                            className="upload-button">
                            Upload
                        </button>
                    </>
                )}
            </form>

            <div className="file-types">
                Plain text (.txt, .csv) <br />
            </div>
        </div>
    );
}

export default FileUpload
