import React from 'react'
import FileUpload from './FileUpload'
import InitialUploadHeader from './InitialUploadHeader'

const EmptyPortfolio = ({ onFileUpload, setIsLoading, isLoading, setUploadFormatFailed, setBadCoins }) => {
  
  return (
    <div>
      <InitialUploadHeader />
      <FileUpload
        onFileUpload={onFileUpload}
        setUploadFormatFailed={setUploadFormatFailed}
        setBadCoins={setBadCoins}
        setIsLoading={setIsLoading}
        isLoading={isLoading}
      />
    </div>
  )
}

export default EmptyPortfolio