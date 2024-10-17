import React from 'react'
import BadUpload from './BadUpload'
import BadUploadHeader from './BadUploadHeader'
import { useState } from 'react'
import FileUpload from './FileUpload'

const EmptyPortfolio = ({ onFileUpload, setIsLoading, isLoading }) => {
  const [uploadFormatFailed, setUploadFormatFailed] = useState(false);
  const [goodCoins, setGoodCoins] = useState([]);
  const [badCoins, setBadCoins] = useState([]);

  return (
    <div className='emptyPortfolio'>
      {uploadFormatFailed ? (
        <>
          <BadUploadHeader />
          <BadUpload
            goodCoins={goodCoins}
            setGoodCoins={setGoodCoins}
            badCoins={badCoins}
            setBadCoins={setBadCoins}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setIsLoading={setIsLoading}
          />
          <FileUpload
            uploadCaption={"File isn't formatted correctly. Please re-upload."}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setBadCoins={setBadCoins}
            setGoodCoins={setGoodCoins}
            setIsLoading={setIsLoading}
            isLoading={isLoading}
          />
        </>) : (
        <>
          <FileUpload
            uploadCaption={"Want to calculate your crypto portfolio value?"}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setBadCoins={setBadCoins}
            setGoodCoins={setGoodCoins}
            setIsLoading={setIsLoading}
            isLoading={isLoading}
          />
        </>)}

    </div>
  )
}

export default EmptyPortfolio