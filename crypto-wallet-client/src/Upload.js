import React from 'react'
import { useState } from 'react';

const Upload = ({ onFileUpload }) => {
    const [file, setFile] = useState(null);
    const upload_ApiUrl = "https://localhost:7038/api/coins/upload";


    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (file) {
            const formData = new FormData();
            formData.append('file', file); // 'file' should match the parameter name in your API

            try {
                const response = await fetch(upload_ApiUrl, {
                    method: 'POST',
                    body: formData,
                });

                if (!response.ok) throw new Error('File upload failed');

                const result = await response.json();
                console.log('File upload successful:', result);
                onFileUpload(result);
            } catch (error) {
                console.error('Error uploading file:', error);
            }
        }
    };

    return (
        <form className='upload-form' onSubmit={handleSubmit}>
            <label htmlFor="file-upload">Upload your crypto portfolio:</label>
            <input
                type="file"
                id="file-upload"
                accept=".txt, .csv" // Accepts text and CSV files
                onChange={handleFileChange}
            />
            <button type="submit">Upload</button>
        </form>
    );
};

export default Upload